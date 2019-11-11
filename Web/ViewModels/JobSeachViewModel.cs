using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using LegacyAPI;

namespace Web.ViewModels
{
    public class JobSeachViewModel
    {
        public string Keyword { get; set; }

        public bool IsRemote { get; set; }

        public List<Domain.Entities.Job> Jobs { get; set; }

        public Domain.Entities.Job Job { get; set; }

        public bool IsPreview { get; set; }
        public IList<JobCardDTO> JobCards { get; internal set; }
    }
}
