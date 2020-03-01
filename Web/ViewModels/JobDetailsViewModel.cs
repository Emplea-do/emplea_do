using System;
using Domain.Entities;
using LegacyAPI;

namespace Web.ViewModels
{
    public class JobDetailsViewModel
    {
        public bool IsPreview { get; set; }

        public Job Job { get; set; }
        public JobCardDTO JobCard {get;set;}
    }
}
