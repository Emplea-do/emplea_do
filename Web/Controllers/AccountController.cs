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
using Web.ViewModels.Account.Facebook;

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
                    var redirectUrl = Url.AbsoluteAction("FacebookCallback", "Account", new { returnUrl });
                    return Redirect($"https://www.facebook.com/v3.1/dialog/oauth?client_id={_socialKeys.FacebookAppId}&redirect_uri={redirectUrl}&state={_socialKeys.LocalVerificationToken}");
            }
            
            return RedirectToAction("Login").WithError(provider);
        }
        public async Task<ActionResult> FacebookCallback(string response_type, string code, string token, string state, string returnUrl)
        {
            if (state == _socialKeys.LocalVerificationToken)
            {
                var redirectUrl = Url.AbsoluteAction("FacebookCallback", "Account", new { returnUrl });
                var url = $"https://graph.facebook.com/v3.1/oauth/access_token?client_id={_socialKeys.FacebookAppId}&redirect_uri={redirectUrl}&client_secret={_socialKeys.FacebookAppSecret}&code={code}";

                using (var client = new HttpClient())
                {
                    try
                    {
                        var strResult = await client.GetStringAsync(url);
                        var result = JsonConvert.DeserializeObject<TokenResponse>(strResult);
                        //TODO: Confirm token and take data
                    }
                    catch(Exception ex)
                    {
                        return RedirectToAction("Login").WithError(ex.Message);
                    }
                }
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
            var user = _userRepository.GetById(_applicationUser.Id);

            return View(user);
        }
    }
}
