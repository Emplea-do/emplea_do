using System;
using AppService.Framework.Social;
using Domain.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Web.Framework
{
    public abstract class BaseController : Controller
    {
        protected ApplicationUser _applicationUser { get; set; }
        protected SocialKeys _socialKeys { get; set; }

        public BaseController(IOptions<SocialKeys> socialKeys)
        {
            _socialKeys = socialKeys.Value;
        }

        public abstract IActionResult Index();
    }
}
