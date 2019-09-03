using System;
using System.Collections.Generic;
using Domain;

namespace Web.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public IEnumerable<Job> Jobs { get; set; }
    }
}
