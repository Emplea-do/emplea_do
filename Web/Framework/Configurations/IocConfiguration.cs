using System;
using AppServices.Services;
using Data;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Scrutor;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Web.Framework.Configurations
{
    public static class IocConfiguration
    {
        public static void Init(IConfiguration configuration, IServiceCollection services)
        {
            services.AddScoped<EmpleaDbContext, EmpleaDbContext>();

            services.Scan(x => x.FromAssemblyOf<EmpleaDbContext>()
                .AddClasses()
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsMatchingInterface()
                .WithScopedLifetime());

            services.Scan(x => x.FromAssemblyOf<IJobsService>()
                .AddClasses()
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsMatchingInterface()
                .WithScopedLifetime());

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAuthentication(options => {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
            })
            .AddGoogle(googleOptions => { 
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
             })
            .AddFacebook(facebookOptions => { 
                facebookOptions.AppId = configuration["Authentication:Facebook:AppId"];
                facebookOptions.AppSecret = configuration["Authentication:Facebook:AppSecret"];
            })
            .AddMicrosoftAccount(microsoftOptions => {
                microsoftOptions.ClientId = configuration["Authentication:Microsoft:ClientId"];
                microsoftOptions.ClientSecret = configuration["Authentication:Microsoft:ClientSecret"];
            })
            .AddLinkedIn(linkedinOptions => {
                linkedinOptions.ClientId = configuration["Authentication:LinkedIn:ClientId"];
                linkedinOptions.ClientSecret = configuration["Authentication:LinkedIn:ClientSecret"];
            })
            .AddGitHub(githubOptions => {
                githubOptions.ClientId = configuration["Authentication:Github:ClientId"];
                githubOptions.ClientSecret = configuration["Authentication:Github:ClientSecret"];
                githubOptions.Scope.Add("user:email");
            });
        }

    }
}
