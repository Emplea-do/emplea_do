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
        private IUsersService _usersService;

        public UserProfileController(IJobsService jobsService, IUsersService usersService)
        {
            _jobsService = jobsService;
            _usersService = usersService;
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