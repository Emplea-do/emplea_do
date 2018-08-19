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

namespace Web.Controllers
{
    public class JobsController : Controller
    {
        private readonly IJobService _jobService;
        private readonly ICategoryService _categoryService;
        private readonly IHireTypeService _hireTypeService;

        public JobsController(IOptions<SocialKeys> socialKeys,
                              IJobService jobsService,
                              ICategoryService categoryService,
                              IHireTypeService hireTypeService) //: base(socialKeys)
        {
            _jobService = jobsService;
            _categoryService = categoryService;
            _hireTypeService = hireTypeService;
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

        //public override IActionResult Index()
        //{
        //    throw new NotImplementedException();
        //}

        //[Authorize]
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
        public IActionResult Wizard(Wizard model)
        {
            // HACK - For some reason the View.WithError is returning a blank page. I'm fully validating this on javascript.
            // Leaving this code for further fix
            if (!ModelState.IsValid)
                return View(model)
                    .WithError("Han ocurrido errores de validación que no permiten continuar el proceso");
            
            var jobOpportunity = model.ToEntity();
            var jobExists = _jobService.GetById(model.Id);
            if (jobExists == null)
            {
                jobOpportunity.Approved = false; // new jobs unapproved by default
                jobOpportunity.UserId = 1; // TODO this is temporary
                var result = _jobService.Create(jobOpportunity);
                if(!result.ExecutedSuccesfully)
                {
                    return View(model)
                        .WithError("Ha ocurrido un problema al momento de registrar la información. Intentalo más tarde");
                }
            }
            //await _slackService.PostNewJobOpportunity(jobOpportunity, Url);

            // TODO adding this in the next PR - Carlos Campos
            //return RedirectToAction(nameof(Detail), new
            //{
            //    id = UrlHelperExtensions.SeoUrl(jobOpportunity.Id, jobOpportunity.Title),
            //    fromWizard = 1
            //});
            return RedirectToAction("Index");
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
