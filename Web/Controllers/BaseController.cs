using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Framework;

namespace Web.Controllers
{
    public class BaseController : Controller
    {
        protected ApplicationUser _currentUser => new ApplicationUser(HttpContext.User);
    }
}
