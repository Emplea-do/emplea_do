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
            switch(provider)
            {
                case "facebook":
                    var redirectUrl = Url.AbsoluteAction("FacebookCallback", "Account");
                    return Redirect($"https://www.facebook.com/v3.1/dialog/oauth?client_id={_socialKeys.FacebookAppId}&state={_socialKeys.LocalVerificationToken}&redirect_uri={redirectUrl}");
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
                    _applicationUser.Init(result.User);
                else
                    RedirectToAction("Login", "Account").WithError(result.Message);
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult LogOff()
        {
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public override IActionResult Index()
        {
            var user = _userRepository.GetById(_applicationUser.RawId);

            return View(user);
        }
    }
}
