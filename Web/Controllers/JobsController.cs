using System;
using Microsoft.AspNetCore.Mvc;
using AppServices;
using AppServices.Services;
using Web.ViewModels;
using Domain;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Web.Framework.Helpers.Alerts;
using Domain.Entities;

namespace Web.Controllers
{
    public class JobsController : BaseController
    {
        private readonly IJobsService _jobsService;
        private readonly ICategoriesService _categoriesService;
        private readonly IHireTypesService _hiretypesService;
        private readonly ITwitterService _twitterService;
        private readonly LegacyApiClient _apiClient;
        private readonly IConfiguration _configuration;
        private readonly ICompaniesService _companiesService;

        public JobsController(IJobsService jobsService, ICategoriesService categoriesService, IHireTypesService hiretypesService, ITwitterService twitterService, LegacyApiClient apiClient, IConfiguration configuration, ICompaniesService companiesService)
        {
            _jobsService = jobsService;
            _categoriesService = categoriesService;
            _hiretypesService = hiretypesService;
            _twitterService = twitterService;
            _apiClient = apiClient;
            _configuration = configuration;
            _companiesService = companiesService;
        }

        public async Task<IActionResult> Index(string keyword = "", bool isRemote = false)
        {
           
            var legacyJobs = await _apiClient.GetJobsFromLegacy();
            var viewModel = new JobSeachViewModel
            {
                Keyword = keyword,
                IsRemote = isRemote,
                JobCards = legacyJobs
            };
            return View(viewModel);
        }

        [Authorize]
        public IActionResult Wizard()
        {
            var model = new WizardViewModel
            {
                Categories = _categoriesService.GetAll(),
                JobTypes = _hiretypesService.GetAll(),
                Companies  = _companiesService.GetByUserId(_currentUser.UserId)
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Wizard(WizardViewModel model)
        {
            model.Categories = _categoriesService.GetAll();
            model.JobTypes = _hiretypesService.GetAll();
            model.Companies = _companiesService.GetByUserId(_currentUser.UserId);

            if (ModelState.IsValid)
            {
                try
                {
                    var companyId = model.CompanyId;
                    if (model.CreateNewCompany)
                    {
                        var company = new Company
                        {
                            Name = model.CompanyName,
                            Url = model.CompanyUrl,
                            LogoUrl = model.CompanyLogoUrl,
                            UserId = _currentUser.UserId,
                            Email = model.CompanyEmail
                        };

                        _companiesService.Create(company);
                        companyId = company.Id;
                    }

                    var newJob = new Job
                    {
                        CategoryId = model.CategoryId,
                        HireTypeId = model.JobTypeId,
                        CompanyId = companyId.Value,
                        HowToApply = model.HowToApply,
                        Description = model.Description,
                        Title = model.Title,
                        IsRemote = model.IsRemote,
                        Location = new Location
                        {
                            PlaceId = model.LocationPlaceId,
                            Name = model.LocationName,
                            Longitude = model.LocationLongitude,
                            Latitude = model.LocationLatitude
                        },
                        UserId = _currentUser.UserId,
                        IsHidden = true,
                        Approved = false
                    };
                    var result = _jobsService.Create(newJob);

                    if(result.Success)
                    {
                        //TODO Llamar al slack service para aprobar la posición
                        return RedirectToAction("Details", new { newJob.Id, isPreview=true } ).WithInfo(result.Messages);
                    }
                    return View(model).WithError(result.Messages);

                }
                catch(Exception ex)
                {
                    return View(model).WithError(ex.Message);
                }
            }


            return View(model);
        }


        public async Task<ActionResult> Details(string Id, bool isPreview)
        {

            if (String.IsNullOrEmpty(Id))
                return RedirectToAction(nameof(this.Index));

            //int jobId = this.GetJobIdFromTitle(Id);

            //if (jobId == 0)
            //    return RedirectToAction(nameof(this.Index));

            int jobId = Int32.Parse(Id);
            var job = await _apiClient.GetJobById(Id);//this._jobsService.GetDetails(jobId, isPreview);

            //Manage error message
            if (job == null)
                return RedirectToAction(nameof(this.Index));

            //If reach this line is because the job exists
            var viewModel = new JobDetailsViewModel
            {
                JobCard = job
            };

            if (isPreview)
            {
                viewModel.IsPreview = isPreview;
                return View(viewModel);
            }
            return View(viewModel);
        }


        private int GetJobIdFromTitle(string title)
        {
            var url = title.Split('-');
            if (String.IsNullOrEmpty(title) || title.Length == 0 || !int.TryParse(url[0], out int id))
                return 0;
            return id;
        }

    }
}
