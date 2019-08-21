using System;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class CreditsController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
