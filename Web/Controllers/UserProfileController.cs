using AppServices.Services;
using Microsoft.AspNetCore.Mvc;
using Web.ViewModels;

namespace Web.Controllers
{
    public class UserProfileController : Controller
    {
        private IJobsService _jobsService;

        public UserProfileController()
        {
            _jobsService = new JobsService();
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