using System;
using System.Collections.Generic;
using Domain;
using Domain.Entities;

namespace Web.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public IEnumerable<Job> Jobs { get; set; }
    }
}
