using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AppServices.Services;
using Domain.Entities;
using ElmahCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Framework.Extensions;
using Web.Framework.Helpers.Alerts;

namespace Web.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        readonly IUsersService _userService;
        readonly ILoginsService _loginService;
        public AccountController(IUsersService userService, ILoginsService loginService)
        {
            _userService = userService;
            _loginService = loginService;
        }

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

            return Challenge(new AuthenticationProperties { RedirectUri = returnUrl }, provider); // Url.Action("OnPostConfirmation", "Account", new { returnUrl, provider } )}, provider);
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

        public async Task<IActionResult> OnPostConfirmation(string returnUrl, string provider)
        {
            
            try
            {
                if (string.IsNullOrWhiteSpace(provider))
                {
                    return SignOut(new AuthenticationProperties { RedirectUri = "/" }, CookieAuthenticationDefaults.AuthenticationScheme).WithError("Ocurrió un error autenticando tu cuenta");
                }
                var loginInfo = _loginService.GetLogin(provider.ToLower(), _currentUser.SocialId);
                if(loginInfo == null) //Create new account
                {
                    var newUser = new User {
                        Email = _currentUser.Email,
                        Name = _currentUser.Name,
                    };
                    var result = _userService.Create(newUser);

                    if (result.Success)
                    {
                        var newLogin = new Login
                        {
                            LoginProvider = provider.ToLower(),
                            ProviderKey = _currentUser.SocialId,
                            UserId = newUser.Id
                        };
                        _loginService.Create(newLogin);
                        HttpContext.Session.SetInt32("UserId", newUser.Id);
                    }
                    else
                        return SignOut(new AuthenticationProperties { RedirectUri = "/" }, CookieAuthenticationDefaults.AuthenticationScheme).WithError(result.Messages);
                }
                else
                {
                    HttpContext.Session.SetInt32("UserId",loginInfo.UserId);
                }
            }
            catch(Exception ex)
            {
                HttpContext.RiseError(ex);
                if (ex.InnerException != null)
                    HttpContext.RiseError(ex.InnerException);

                return SignOut(new AuthenticationProperties { RedirectUri = "/" }, CookieAuthenticationDefaults.AuthenticationScheme).WithError(ex.Message);
            }
            returnUrl = returnUrl ?? Url.Content("~/");

            return Redirect(returnUrl);
        }
    }
}