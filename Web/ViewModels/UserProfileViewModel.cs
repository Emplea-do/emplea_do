using System.Security.AccessControl;
using Domain;
using Domain.Entities;
using System.Collections.Generic;

namespace Web.ViewModels
{
    public class UserProfileViewModel : BaseViewModel
    {
        public Company Company { get; set; }
        public List<Job> Jobs { get; set; }
        public List<Company> Companies { get; set; }
        public List<Banner> Banners { get; set; }
    }
}
