using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using System.Threading.Tasks;
using AppServices.Services;
using Microsoft.FeatureManagement;
using System;
using Domain.Entities;

namespace Web.Framework.Configurations
{
    public class AuthConfiguration
    {
        public static void Init(IConfiguration configuration, IServiceCollection services)
        {

            var featureManager = services.BuildServiceProvider().GetService<IFeatureManager>();
            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.SlidingExpiration = true;
            });
            if(featureManager.IsEnabledAsync(FeatureFlags.UseGoogleAuthentication).Result)
                services.AddAuthentication().AddGoogle(googleOptions =>
                {
                    googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
                    googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
                    googleOptions.UserInformationEndpoint = "https://www.googleapis.com/oauth2/v2/userinfo";
                    googleOptions.ClaimActions.Clear();
                    googleOptions.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                    googleOptions.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                    googleOptions.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
                    googleOptions.ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");
                    googleOptions.ClaimActions.MapJsonKey("urn:google:profile", "link");
                    googleOptions.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
                    googleOptions.SaveTokens = true;
                    googleOptions.Events.OnCreatingTicket = ctx => {
                        return ProcessUser(ctx, "Google", services);
                    };
                });
            
            if(featureManager.IsEnabledAsync(FeatureFlags.UseFacebookAuthentication).Result)
                services.AddAuthentication().AddFacebook(facebookOptions =>
                {
                    facebookOptions.AppId = configuration["Authentication:Facebook:AppId"];
                    facebookOptions.AppSecret = configuration["Authentication:Facebook:AppSecret"];
                    facebookOptions.Events.OnCreatingTicket = ctx => {
                        return ProcessUser(ctx, "Facebook", services);
                    };
                });
            if(featureManager.IsEnabledAsync(FeatureFlags.UseMicrosoftAuthentication).Result)
                services.AddAuthentication().AddMicrosoftAccount(microsoftOptions =>
                {
                    microsoftOptions.ClientId = configuration["Authentication:Microsoft:ClientId"];
                    microsoftOptions.ClientSecret = configuration["Authentication:Microsoft:ClientSecret"];
                    microsoftOptions.Events.OnCreatingTicket = ctx => {
                        return ProcessUser(ctx, "Microsoft", services);
                    };
                });
            if(featureManager.IsEnabledAsync(FeatureFlags.UseLinkedInAuthentication).Result)
                services.AddAuthentication().AddLinkedIn(linkedinOptions =>
                {
                    linkedinOptions.ClientId = configuration["Authentication:LinkedIn:ClientId"];
                    linkedinOptions.ClientSecret = configuration["Authentication:LinkedIn:ClientSecret"];
                    linkedinOptions.Events.OnCreatingTicket = ctx => {
                        return ProcessUser(ctx, "linkedin", services);
                    };
                });
            if(featureManager.IsEnabledAsync(FeatureFlags.UseGithubAuthentication).Result)
                services.AddAuthentication().AddGitHub(githubOptions =>
                    {
                    githubOptions.ClientId = configuration["Authentication:Github:ClientId"];
                    githubOptions.ClientSecret = configuration["Authentication:Github:ClientSecret"];
                    githubOptions.Scope.Add("user:email");
                    githubOptions.Events.OnCreatingTicket = ctx => {
                        return ProcessUser(ctx, "github", services);
                    };
                });
        }


        public static Task ProcessUser(OAuthCreatingTicketContext ctx, string provider, IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var loginService = serviceProvider.GetService<ILoginsService>();
            var userService = serviceProvider.GetService<IUsersService>();

            var currentUser = ctx.Identity;
            var socialId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            var loginInfo = loginService.GetLogin(provider.ToLower(), socialId);
            if (loginInfo == null) //Create new account
            {
                var newUser = new User
                {
                    Email = currentUser.FindFirst(ClaimTypes.Email).Value,
                    Name = currentUser.FindFirst(ClaimTypes.Name).Value,
                };
                var result = userService.Create(newUser);

                if (result.Success)
                {
                    var newLogin = new Login
                    {
                        LoginProvider = provider.ToLower(),
                        ProviderKey = socialId,
                        UserId = newUser.Id
                    };
                    loginService.Create(newLogin);
                    var userIdClaim = new Claim("UserId", newUser.Id.ToString());
                    ctx.Identity.AddClaim(userIdClaim);
                }
            }
            else
            {
                var userIdClaim = new Claim("UserId", loginInfo.UserId.ToString());
                ctx.Identity.AddClaim(userIdClaim);
            }
            return Task.CompletedTask;
        }
    }
}