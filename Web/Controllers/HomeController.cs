using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AppServices.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Web.ViewModels;

namespace Web.Controllers
{
    public class HomeController : BaseController
    {
        private IJobsService _jobsService;
        private readonly LegacyApiClient apiClient;

        public HomeController(IJobsService jobsService, LegacyApiClient apiClient)
        {
            _jobsService = jobsService;
            this.apiClient = apiClient;
        }

        public async Task<IActionResult> Index()
        {
            //var recentJobs = _jobsService.GetRecentJobs();
            var jobCards = await apiClient.GetJobsFromLegacy();

            var x = _currentUser;
            var viewModel = new HomeViewModel
            {
               // Jobs = recentJobs,
                JobCards = jobCards
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
