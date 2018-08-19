using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AppService.Framework.Social;
using AppService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Web.Framework;
using Web.ViewModels;

namespace Web.Controllers
{
    public class HomeController : BaseController
    {
        IJobService _jobService;

        public HomeController(IOptions<SocialKeys> socialKeys, IJobService jobsService) : base(socialKeys)
        {
            _jobService = jobsService;
        }

        public IActionResult Index()
        {
            ViewBag.SearchViewModel = new JobSearchViewModel()
            {
                CategoriesCount = _jobService.GetJobCountByCategory()
            };

            var model = _jobService.GetLatestJobs(7);

            return View(model);
        }
    }
}