using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AppService.Framework.Social;
using AppService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Web.Framework;

namespace Web.Controllers
{
    public class HomeController : BaseController
    {
        IJobsService _jobsService;

        public override IActionResult Index()
        {
            return View();
        }

        public HomeController(IOptions<SocialKeys> socialKeys, IJobsService jobsService) : base(socialKeys)
        {
            _jobsService = jobsService;
        }
    }
}