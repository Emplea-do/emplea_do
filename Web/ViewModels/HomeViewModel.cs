using System.Collections.Generic;
using Domain.Entities;
using LegacyAPI;

namespace Web.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public IEnumerable<Job> Jobs { get; set; }
        public IEnumerable<JobCardDTO> JobCards { get; internal set; }

        public List<Category> Categories { get; set; }
    }
}
