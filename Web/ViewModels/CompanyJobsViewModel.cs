using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class CompanyJobsViewModel
    {
        public List<Job> Jobs { get; set; }
        public Company Company { get; set; }
    }
}
