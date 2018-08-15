using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Repositories;
using AppService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Web.Framework.Configurations;
using Web.Framework.Extensions;
using Domain;
using AppService.Framework.Social;
using Web.Framework.Filters;
using AppService.Services.Social;
using Sakura.AspNetCore.Mvc;
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
            //Social network keys
            var socialKeys = Configuration.GetSection("SocialKeys");
            services.Configure<SocialKeys>(socialKeys);
            services.AddSingleton(Configuration);

            // Add connection string to DbContext
            services.AddDbContext<EmpleadoDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IFacebookService, FaceBookService>(x => new FaceBookService(socialKeys.GetValue<string>("FacebookAppId"), socialKeys.GetValue<string>("FacebookAppSecret")));
            services.AddScoped<ILinkedinService, LinkedinService>(x => new LinkedinService(socialKeys.GetValue<string>("LinkedInClientId"), socialKeys.GetValue<string>("LinkedInClientSecret")));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<ILoginService, LoginService>();
            #if DEBUG
            services.AddDbContext<EmpleadoDbContext>(options =>
                options.UseSqlite($"Data Source=../mydb.db")
            );
            
            #else
                services.AddDbContext<EmpleadoDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            #endif

            services.AddIdentity<User, Role>();

            services.ConfigureApplicationCookie(options =>
             {
                 // Cookie settings
                 options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                 options.LoginPath = "/Account/Login";
                 options.AccessDeniedPath = "/Error/401";
                 options.SlidingExpiration = true;
            });

            // Add default bootstrap-styled pager implementation
            services.AddBootstrapPagerGenerator(options =>
            {
                // Use default pager options.
                options.ConfigureDefault();
             });
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(options => {
                    options.LoginPath = "/Account/Login/";
                    options.AccessDeniedPath = "/Error/401";
                    options.ExpireTimeSpan = TimeSpan.FromDays(30);
                });
            services.AddDbContext<EmpleadoDbContext>();

            services.AddSession();
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(UnderMaintenanceFilterAttribute));
            }).AddSessionStateTempDataProvider();

            IocConfiguration.Init(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseStatusCodePagesWithRedirects("/error/{0}");
                app.UseExceptionHandler();
                app.UseHsts();
            }
            app.ConfigureEnvironment(env);
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.ConfigureRoutes();
            app.UseMvc();
        }
    }
}
