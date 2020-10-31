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
        private ICompaniesService _companiesService;
        private IBannersService _bannersService;

        public UserProfileController(IJobsService jobsService, IUsersService usersService, 
        ICompaniesService companiesService, IBannersService bannersService)
        {
            _jobsService = jobsService;
            _usersService = usersService;
            _companiesService = companiesService;
            _bannersService = bannersService;
        }

        public IActionResult Index()
        {
            var filteredJobsByUserProfile = _jobsService.GetByUser(_currentUser.UserId);
            var filteredCompaniesByUserProfile = _companiesService.GetByUserId(_currentUser.UserId);
            var filteredBannersByUserProfile = _bannersService.GetByUserId(_currentUser.UserId);

            var viewModel = new UserProfileViewModel
            {
                Jobs = filteredJobsByUserProfile,
                Companies = filteredCompaniesByUserProfile,
                Banners = filteredBannersByUserProfile
            };

            return View(viewModel);
        }
    }
}