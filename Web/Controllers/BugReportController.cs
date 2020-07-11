using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Web.Framework.Helpers.Alerts;
using Web.ViewModels;
using Web.Services.ReCaptcha;
using Newtonsoft.Json;
using Web.Framework;

namespace Web.Controllers
{
    public class BugReportController : BaseController
    {
        private readonly string _siteKey;
        private readonly IReCaptchaService _recaptchaService;

        public BugReportController(IConfiguration configuration, IReCaptchaService reCaptchaService)
        {
            _siteKey = configuration["GoogleReCaptcha:SiteKey"];
            _recaptchaService = reCaptchaService;
        }
        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            ViewData["ReCaptchaKey"] = _siteKey;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Index(BugReportViewModel model)
        {
            ViewData["ReCaptchaKey"] = _siteKey;

            if (ModelState.IsValid)
            {
                if (!_recaptchaService.ReCaptchaPassed(Request.Form["g-recaptcha-response"]))
                {
                    ModelState.AddModelError(string.Empty, "Ha fallado captcha, creemos que no eres un humano.");
                    return View(model);
                }

                // do your stuff with the model
                // ...

                return View();
            }

            return View(model);
        }
    }
}