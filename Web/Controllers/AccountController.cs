using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Web.Framework.Extensions;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet("/Account/Login")]
        public async Task<IActionResult> Login(string returnUrl = "/")
        {
            if (HttpContext.User != null && HttpContext.User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            ViewBag.ReturnUrl = returnUrl;
            return View("Login", await HttpContext.GetExternalProvidersAsync());
        }

        [HttpPost("/Account/Login")]
        public async Task<IActionResult> Login([FromForm] string provider, [FromForm] string returnUrl)
        {
            if (string.IsNullOrWhiteSpace(provider))
            {
                return BadRequest();
            }

            if (!await HttpContext.IsProviderSupportedAsync(provider))
            {
                return BadRequest();
            }

            return Challenge(new AuthenticationProperties { RedirectUri = returnUrl }, provider);
        }

        [HttpGet]
        public IActionResult HandleExternalLogin()
        {
            return Redirect("/");
        }

        [HttpGet("/Account/Logout"), HttpPost("/Account/Logout")]
        public IActionResult LogOut()
        {
            return SignOut(new AuthenticationProperties { RedirectUri = "/" },
                CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}