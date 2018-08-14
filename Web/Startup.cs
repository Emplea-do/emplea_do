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
using Sakura.AspNetCore.Mvc;

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
            services.Configure<SocialKeys>(Configuration.GetSection("SocialKeys"));
            services.AddSingleton<IConfiguration>(Configuration);

            // Add connection string to DbContext
            services.AddDbContext<EmpleadoDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            #if DEBUG
            services.AddDbContext<EmpleadoDbContext>(options =>
                options.UseSqlite($"Data Source=../mydb.db")
            );
            
            #else
                services.AddDbContext<EmpleadoDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            #endif

            services.AddIdentity<User, Role>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Add default bootstrap-styled pager implementation
            services.AddBootstrapPagerGenerator(options =>
            {
                // Use default pager options.
                options.ConfigureDefault();
            });

            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddMvc(options=> {
                options.Filters.Add(typeof(UnderMaintenanceFilterAttribute));
            }).AddSessionStateTempDataProvider();

            services.AddDbContext<EmpleadoDbContext>();

            IocConfiguration.Init(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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
        }
    }
}
