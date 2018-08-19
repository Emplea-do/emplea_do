using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AppService.Framework.Social;
using Domain.Framework;
using Domain.Framework.Dto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Web.Framework
{
    public abstract class BaseController : Controller
    {
        protected ApplicationUser _applicationUser => new ApplicationUser(User);
        protected SocialKeys _socialKeys { get; set; }

        public BaseController(IOptions<SocialKeys> socialKeys)
        {
            _socialKeys = socialKeys.Value;
        }

        public abstract IActionResult Index();

        protected async Task SignIn(UserLimited user)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.Email, ClaimValueTypes.String),
                //TODO: Add name
                new Claim("Email", user.Email, ClaimValueTypes.String),
                new Claim("Id", user.Id.ToString(), ClaimValueTypes.String),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await AuthenticationHttpContextExtensions.SignInAsync(HttpContext, CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        protected async Task SignOut()
        {
            await AuthenticationHttpContextExtensions.SignOutAsync(HttpContext, CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
