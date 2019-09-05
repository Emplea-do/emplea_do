using System;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class AccountController : Controller
    {

        public IActionResult Login(){
            return View();
        }
    }
}