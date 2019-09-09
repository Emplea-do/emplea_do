using AppServices.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.ViewModels;

namespace Web.Controllers
{
    [Authorize]
    public class UserProfileController : BaseController
    {
        private IJobsService _jobsService;

        public UserProfileController(IJobsService jobsService)
        {
            _jobsService = jobsService;
        }

        public IActionResult Index()
        {
            var filteredJobsByUserProfile = _jobsService.GetByUser(_currentUser.UserId);
            var viewModel = new UserProfileViewModel
            {
                Jobs = filteredJobsByUserProfile
            };
            return View(viewModel);
        }
    }
}