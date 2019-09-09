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

            // Instruct the middleware corresponding to the requested external identity
            // provider to redirect the user agent to its own authorization endpoint.
            // Note: the authenticationScheme parameter must match the value configured in Startup.cs
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
            // Instruct the cookies middleware to delete the local cookie created
            // when the user agent is redirected from the external identity provider
            // after a successful authentication flow (e.g Google or Facebook).
            return SignOut(new AuthenticationProperties { RedirectUri = "/" },
                CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}