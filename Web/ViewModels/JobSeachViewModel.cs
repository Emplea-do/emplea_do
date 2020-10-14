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

        public int? CategoryId { get; set; }

        public int? HireTypeId { get; set; }

        // public bool IsPreview { get; set; }
        // public IList<JobCardDTO> JobCards { get; internal set; }
        public IEnumerable<Job> Jobs { get; set; }

        public List<Category> Categories { get; set; }
        public List<HireType> HireTypes { get; set; }
    }
}
