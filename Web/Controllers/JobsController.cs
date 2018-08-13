using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppService.Framework.Social;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Web.Framework;
using Web.ViewModels.Jobs;

namespace Web.Controllers
{
    public class JobsController : BaseController
    {
        public JobsController(IOptions<SocialKeys> socialKeys) : base(socialKeys)
        {
        }

        public override IActionResult Index()
        {
            throw new NotImplementedException();
        }

        [Authorize]
        public IActionResult New()
        {
            return RedirectToAction("Wizard");
        }

        [HttpGet]
        [Authorize]
        public ActionResult Wizard()
        {
            var viewModel = new Wizard();
            viewModel.MapsApiKey = _socialKeys.GoogleMapsApiKey;
            return View(viewModel);
        }
    }
}
