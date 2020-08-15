using System;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class CreditsController : BaseController
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
