using System;
using System.Collections.Generic;
using Domain;
using Domain.Entities;

namespace Web.ViewModels
{
    public class JobsViewModel : BaseViewModel
    {
        public string Keyword { get; set; }

        public bool IsRemote { get; set; }

        public List<Job> Jobs { get; set; }

        public Job Job { get; set; }

        public bool IsPreview { get; set; }
    }
}
