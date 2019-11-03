using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using System.Threading.Tasks;
using AppServices.Services;
using Microsoft.FeatureManagement;

namespace Web.Framework.Configurations
{
    public class AuthConfiguration
    {
        public static void Init(IConfiguration configuration, IServiceCollection services, IFeatureManager featureManager )
        {
            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.SlidingExpiration = true;
            });
            
            if(featureManager.IsEnabled(FeatureFlags.UseGoogleAuthentication))
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
            });
            
            if(featureManager.IsEnabled(FeatureFlags.UseFacebookAuthentication))
                services.AddAuthentication().AddFacebook(facebookOptions =>
                {
                    facebookOptions.AppId = configuration["Authentication:Facebook:AppId"];
                    facebookOptions.AppSecret = configuration["Authentication:Facebook:AppSecret"];
                });
            
            if(featureManager.IsEnabled(FeatureFlags.UseMicrosoftAuthentication))
                services.AddAuthentication().AddMicrosoftAccount(microsoftOptions =>
                {
                    microsoftOptions.ClientId = configuration["Authentication:Microsoft:ClientId"];
                    microsoftOptions.ClientSecret = configuration["Authentication:Microsoft:ClientSecret"];
                });

            if(featureManager.IsEnabled(FeatureFlags.UseLinkedInAuthentication))
                services.AddAuthentication().AddLinkedIn(linkedinOptions =>
                {
                    linkedinOptions.ClientId = configuration["Authentication:LinkedIn:ClientId"];
                    linkedinOptions.ClientSecret = configuration["Authentication:LinkedIn:ClientSecret"];
                });
            
            if(featureManager.IsEnabled(FeatureFlags.UseGithubAuthentication))
                services.AddAuthentication().AddGitHub(githubOptions =>
                    {
                    githubOptions.ClientId = configuration["Authentication:Github:ClientId"];
                    githubOptions.ClientSecret = configuration["Authentication:Github:ClientSecret"];
                    githubOptions.Scope.Add("user:email");
                });
        }
    }
}