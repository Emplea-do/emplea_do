using System;
using Microsoft.AspNetCore.Mvc;
using AppServices;
using AppServices.Services;
using Web.ViewModels;

namespace Web.Controllers
{
    public class JobsController : Controller
    {
        private IJobsService _jobsService;

        public JobsController(IJobsService jobsService)
        {
            _jobsService = jobsService;
        }

        public IActionResult Index(string keyword = "", bool isRemote = false)
        {
            var filteredJobs = _jobsService.GetAll();
            var viewModel = new JobsViewModel
            {
                Keyword = keyword,
                IsRemote = isRemote,
                Jobs = filteredJobs
            };
            return View(viewModel);
        }


        public IActionResult Details()
        {
            return View();
        }
    }
}
