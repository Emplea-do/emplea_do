using Domain;
using Domain.Entities;
using System.Collections.Generic;

namespace Web.ViewModels
{
    public class UserProfileViewModel : BaseViewModel
    {
        public Company Company { get; set; }
        public List<Job> Jobs { get; set; }

    }
}
