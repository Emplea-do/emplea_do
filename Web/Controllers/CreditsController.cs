using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Framework;

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
