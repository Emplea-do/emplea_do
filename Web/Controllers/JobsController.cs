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

namespace Web.Controllers
{
    public class JobsController : Controller
    {
        private readonly IJobService _jobService;

        public JobsController(IOptions<SocialKeys> socialKeys,
                              IJobService jobsService) //: base(socialKeys)
        {
            _jobService = jobsService;
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

            var jobs = _jobService.GetAllJobOpportunitiesPagedByFilters(model);

            viewModel.Jobs = jobs.ToPagedList(model.PageSize, model.Page);

            return View(viewModel);
        }

        //public override IActionResult Index()
        //{
        //    throw new NotImplementedException();
        //}

        [Authorize]
        public IActionResult New()
        {
            return RedirectToAction("Wizard");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Wizard()
        {
            var viewModel = new Wizard();
            //viewModel.MapsApiKey = _socialKeys.GoogleMapsApiKey;
            return View(viewModel);
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
