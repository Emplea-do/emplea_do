using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppService.Framework.Social;
using AppService.Services;
using Domain.Framework.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Web.Framework;
using Web.Framework.Helpers.Alerts;
using Web.ViewModels;
using Web.ViewModels.Jobs;
using Sakura.AspNetCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using AppService.Framework;
using Domain;
using Web.Framework.Extensions;
using Microsoft.AspNetCore.Mvc.Routing;
using AppService.Services.Social;

namespace Web.Controllers
{
    public class JobsController : BaseController
    {
        private readonly IJobService _jobService;
        private readonly ICategoryService _categoryService;
        private readonly IHireTypeService _hireTypeService;
        private readonly ISlackService _slackService;

        public JobsController(IOptions<SocialKeys> socialKeys,
                              IJobService jobsService,
                              ICategoryService categoryService,
                              IHireTypeService hireTypeService,
                              ISlackService slackServie) : base(socialKeys)
        {
            _jobService = jobsService;
            _categoryService = categoryService;
            _hireTypeService = hireTypeService;
            _slackService = slackServie;
        }

        public IActionResult Index(JobPagingParameter model)
        {
            var viewModel = GetSearchViewModel(model);

            if (!string.IsNullOrWhiteSpace(viewModel.SelectedLocationName) &&
                string.IsNullOrWhiteSpace(viewModel.SelectedLocationPlaceId))
            {
                ModelState.AddModelError("SelectedLocationName", "");
                return View(viewModel).WithError("Debe seleccionar una Localidad para buscar.");
            }

            var jobs = _jobService.GetAllJobsPagedByFilters(model);

            viewModel.Jobs = jobs.ToPagedList(model.PageSize, model.Page);

            return View(viewModel);
        }

        [Authorize]
        public IActionResult New()
        {
            return RedirectToAction("Wizard");
        }

        [HttpGet]
        //[Authorize]
        public IActionResult Wizard()
        {
            var viewModel = new Wizard();
            var categories = _categoryService.GetCategories().ToList();
            var jobtypes = _hireTypeService.GetHireTypes().ToList();
            viewModel.Categories = categories;
            viewModel.JobTypes = jobtypes;
            //viewModel.MapsApiKey = _socialKeys.GoogleMapsApiKey;
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        //[ValidateInput(false)]
        //[CaptchaValidator(RequiredMessage = "Por favor confirma que no eres un robot", ErrorMessage = "El captcha es incorrecto.")]
        public async Task<IActionResult> WizardAsync(Wizard model)
        {
            // HACK - For some reason the View.WithError is returning a blank page. I'm fully validating this on javascript.
            // Leaving this code for further fix
            if (!ModelState.IsValid)
                return View(model)
                    .WithError("Han ocurrido errores de validación que no permiten continuar el proceso");
            
            var job = model.ToEntity();
            var jobExists = _jobService.GetById(model.Id);
            if (jobExists == null)
            {
                job.Approved = false; // new jobs unapproved by default
                job.UserId = _applicationUser.RawId; // TODO this is temporary
                var result = _jobService.Create(job);
                if(!result.ExecutedSuccesfully)
                {
                    return View(model)
                        .WithError("Ha ocurrido un problema al momento de registrar la información. Intentalo más tarde");
                }
            }
            var seoUrl = UrlExtensions.SeoUrl(job.Id, job.Title);
            var url = Url.AbsoluteAction(seoUrl, "jobs");


            await _slackService.PostNewJob(job, url);

            // TODO adding this in the next PR - Carlos Campos
            //return RedirectToAction(nameof(Detail), new
            //{
            //    id = UrlHelperExtensions.SeoUrl(job.Id, job.Title),
            //    fromWizard = 1
            //});
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult ToggleHide(string title)
        {
            var job = GetJobOpportunityFromTitle(title);
            if (IsJobOpportunityOwner(title))
            {
                _jobService.ToggleHideState(job);
            }

            return Json(new { isHidden = job.IsHidden });

        }


        private static int GetIdFromTitle(string title)
        {
            int id;
            var url = title.Split('-');

            if (string.IsNullOrEmpty(title) || url.Length == 0 || !int.TryParse(url[0], out id))
                return 0;

            return id;
        }

        private Job GetJobOpportunityFromTitle(string title)
        {
            var jobId = GetIdFromTitle(title);
            return _jobService.GetById(jobId);
        }

        private bool IsJobOpportunityOwner(string title)
        {
            var job = GetJobOpportunityFromTitle(title);
            var currentUser = _applicationUser.RawId;
            return (currentUser != null && job.UserId == currentUser);
        }


        /// <summary>
        /// Transform JobOpportunityPagingParameter into JobOpportunitySearchViewModel with Locations
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private JobSearchViewModel GetSearchViewModel(JobPagingParameter model)
        {
            if (string.IsNullOrWhiteSpace(model.SelectedLocationName))
            {
                model.SelectedLocationLatitude = default(double);
                model.SelectedLocationLongitude = default(double);
                model.SelectedLocationPlaceId = string.Empty;
            }

            var viewModel = new JobSearchViewModel
            {
                SelectedLocationPlaceId = model.SelectedLocationPlaceId,
                SelectedLocationName = model.SelectedLocationName,
                SelectedLocationLongitude = model.SelectedLocationLongitude,
                SelectedLocationLatitude = model.SelectedLocationLatitude,
                CategoryId = model.CategoryId,
                Keyword = model.Keyword,
                IsRemote = model.IsRemote,
                CategoriesCount = _jobService.GetJobCountByCategory()
            };

            return viewModel;
        }
    }
}
