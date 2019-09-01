using System;
using Microsoft.AspNetCore.Mvc;
using AppServices;
using AppServices.Services;
using Web.ViewModels;
using Domain;

namespace Web.Controllers
{
    public class JobsController : Controller
    {
        private IJobsService _jobsService;

        public JobsController()
        {
            _jobsService = new JobsService();
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


        public IActionResult Details(string Id, bool isPreview)
        {

            if (String.IsNullOrEmpty(Id))
                return RedirectToAction(nameof(this.Index));


            int jobId = this.GetJobIdFromTitle(Id);

            if (jobId == 0)
                return RedirectToAction(nameof(this.Index));

            var job = this._jobsService.GetDetails(jobId, isPreview);

            //Manage error message
            if (job == null)
                return RedirectToAction(nameof(this.Index));
            
            //If reach this line is because the job exists
            var viewModel = new JobsViewModel
            {
                Job = job
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

        private bool IsPreview(Job job)
        {
            var fakeCurrentUser = new { UserId = 10 };

            Boolean isPreview = job.UserId.Equals(fakeCurrentUser.UserId) && !job.Approved;
            return isPreview;
        }
    }
}
