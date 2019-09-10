using System;
using Domain.Entities;

namespace Web.ViewModels
{
    public class JobDetailsViewModel
    {
        public bool IsPreview { get; set; }

        public Job Job { get; set; }
    }
}
