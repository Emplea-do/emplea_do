using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Domain.Entities;
using LegacyAPI;

namespace Web.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public IEnumerable<Job> Jobs { get; set; }
        public IEnumerable<JobCardDTO> JobCards { get; internal set; }
    }
}
