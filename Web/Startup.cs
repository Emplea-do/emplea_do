using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppServices.Data;
using ElmahCore;
using ElmahCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;
using Web.Framework.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddApplicationInsightsTelemetry();
            services.AddFeatureManagement();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                //options.CheckConsentNeeded = context => true;
                // options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            // Registers the standard IFeatureManager implementation, which utilizes the .NET Standard configuration system.
            //Read more https://andrewlock.net/introducing-the-microsoft-featuremanagement-library-adding-feature-flags-to-an-asp-net-core-app-part-1/

#if DEBUG
            services.AddDbContext<EmpleaDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            #else
                services.AddDbContext<EmpleaDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            #endif

            services.Configure<AppServices.Services.TwitterConfig>(Configuration.GetSection("TwitterConfig"));
            services.Configure<LegacyApiClient>(Configuration);


            IocConfiguration.Init(Configuration, services);
            AuthConfiguration.Init(Configuration, services);

            services.AddElmah<XmlFileErrorLog>(options => {
                options.LogPath = "~/Helpers/log";
                options.Path = "ErrorLogs";
                options.CheckPermissionAction = context => context.User.Identity.IsAuthenticated;
            });

            services.Configure<IISServerOptions>(options => {
                options.AllowSynchronousIO = true;
            });
            services.AddSession();
            //services.AddMvc();//option => option.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            Console.WriteLine("Startup.ConfigureServices() End");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            Console.WriteLine("Startup.Configure() Begin");

            #if DEBUG
                app.UseDeveloperExceptionPage();
            #else
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            #endif
            app.UseAzureAppConfiguration();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.UseElmah();

            app.UseEndpoints(routes =>
            {
                routes.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
