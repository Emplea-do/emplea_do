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
    public class HomeController : Controller
    {
        private IJobsService _jobsService;

        public HomeController()
        {
            _jobsService = new JobsService();
        }

        public IActionResult Index()
        {
            var recentJobs = _jobsService.GetRecentJobs();
            var viewModel = new HomeViewModel
            {
                Jobs = recentJobs
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
