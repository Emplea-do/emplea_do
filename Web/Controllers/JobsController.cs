using System;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class JobsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Details()
        {
            return View();
        }
    }
}
