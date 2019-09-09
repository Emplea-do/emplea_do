using AppServices.Services;
using Microsoft.AspNetCore.Mvc;
using Web.ViewModels;

namespace Web.Controllers
{
    public class UserProfileController : BaseController
    {
        private IJobsService _jobsService;

        public UserProfileController(IJobsService jobsService)
        {
            _jobsService = jobsService;
        }

        public IActionResult Index(int id)
        {
            var filteredJobsByUserProfile = _jobsService.GetByUserProfile(id);
            var viewModel = new UserProfileViewModel
            {
                Jobs = filteredJobsByUserProfile
            };
            return View(viewModel);
        }
    }
}