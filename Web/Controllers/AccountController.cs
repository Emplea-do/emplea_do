using System;
using System.Net.Http;
using System.Threading.Tasks;
using AppService.Framework.Social;
using AppService.Services;
using Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Web.Framework;
using Web.Framework.Extensions;
using Web.Framework.Helpers.Alerts;

namespace Web.Controllers
{
    public class AccountController : BaseController
    {
        IUserRepository _userRepository;
        ISecurityService _securityService;

        public AccountController(IOptions<SocialKeys> socialKeys, ISecurityService securityService, IUserRepository userRepository) : base(socialKeys)
        {
            _userRepository = userRepository;
            _securityService = securityService;
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login()
        {
            //Confirm the credentials
            //Redirect
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = string.Empty;
            switch(provider)
            {
                case "facebook":
                    redirectUrl = Url.AbsoluteAction("FacebookCallback", "Account");
                    return Redirect($"https://www.facebook.com/v3.1/dialog/oauth?client_id={_socialKeys.FacebookAppId}&state={_socialKeys.LocalVerificationToken}&redirect_uri={redirectUrl}");
                case "google":
                    redirectUrl = Url.AbsoluteAction("GoogleCallback", "Account");
                    return Redirect($"https://accounts.google.com/o/oauth2/v2/auth?response_type=code&client_id={_socialKeys.GoogleClientId}&state={_socialKeys.LocalVerificationToken}&redirect_uri={redirectUrl}&scope=https://www.googleapis.com/auth/userinfo.profile%20https://www.googleapis.com/auth/plus.me%20https://www.googleapis.com/auth/userinfo.email");  
                case "linkedin":
                    redirectUrl = Url.AbsoluteAction("LinkedinCallback", "Account");
                    return Redirect($"https://www.linkedin.com/oauth/v2/authorization?response_type=code&client_id={_socialKeys.LinkedInClientId}&state={_socialKeys.LocalVerificationToken}&redirect_uri={redirectUrl}");
            
            }
            
            return RedirectToAction("Login").WithError(provider);
        }

        public async Task<ActionResult> FacebookCallback(string code, string state, string returnUrl)
        {
            if (state == _socialKeys.LocalVerificationToken)
            {
                var redirectUrl = Url.AbsoluteAction("FacebookCallback", "Account");
                var result = await _securityService.FacebookLogin(code, redirectUrl);
                if (result.ExecutedSuccesfully)
                {
                    await SignIn(result.User);
                }
                else
                {
                    return RedirectToAction("Login", "Account").WithError(result.Message);
                }
            }
            return RedirectToAction("New", "Job");
        }

        public async Task<ActionResult> LinkedinCallback(string code, string state, string returnUrl)
        {
            if (state == _socialKeys.LocalVerificationToken)
            {
                var redirectUrl = Url.AbsoluteAction("LinkedinCallback", "Account");
                var result = await _securityService.LinkedInLogin(code, redirectUrl);
                if (result.ExecutedSuccesfully)
                {
                    await SignIn(result.User);
                }
                else
                {
                    return RedirectToAction("Login", "Account").WithError(result.Message);
                }
            }
            return RedirectToAction("New", "Job");
        }


        public async Task<ActionResult> GoogleCallback(string code, string state, string returnUrl)
        {
            if (state == _socialKeys.LocalVerificationToken)
            {
                var redirectUrl = Url.AbsoluteAction("GoogleCallback", "Account");
                var result = await _securityService.GoogleLogin(code, redirectUrl);
                if (result.ExecutedSuccesfully)
                {
                    await SignIn(result.User);
                }
                else
                {
                    return RedirectToAction("Login", "Account").WithError(result.Message);
                }
            }
            return RedirectToAction("New", "Job");
        }



        public async Task<ActionResult> LogOff()
        {
            await SignOut();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public override IActionResult Index()
        {
            return RedirectToAction("Profile");
        }

        [Authorize]
        public IActionResult Profile()
        {
            var user = _userRepository.GetById(_applicationUser.RawId);
            return View(user);
        }
    }
}
