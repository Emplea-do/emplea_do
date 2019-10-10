using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;


namespace Web
{
    public class Program
    {
        public static IHostingEnvironment HostingEnvironment;
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
           .ConfigureAppConfiguration((hostingContext, config) =>
            {
                HostingEnvironment = hostingContext.HostingEnvironment;
                if(hostingContext.HostingEnvironment.IsDevelopment())
                {
                    // For Development, the already configured provider(s) for App Settings are:
                    // (1) Appsettings.json files and (2) secure secrets (secrets.json) 
                }

                if (hostingContext.HostingEnvironment.IsProduction())
                {
                   // For the production environment, we are going to use 
                   //Azure App Configuration as our provider of (1) App Configuration and (2) Feature Flags
                    var settings = config.Build();
                    var connectionString = settings["AzureAppConfigurationConnectionString"];
                    if (!String.IsNullOrWhiteSpace(connectionString))
                    {
                        config.AddAzureAppConfiguration(options =>

                        {
                            options.Connect(connectionString).UseFeatureFlags();
                            options.ConfigureRefresh(r => r.SetCacheExpiration(TimeSpan.FromSeconds(5)));
                        });
                    }
                    else
                    {
                        // Something went terribly wrong, this environment does not have Azure App Configuration setup
                        throw new Exception("No connection string to Azure App Configuration service found.");
                    }
                }
        }).UseStartup<Startup>();


    }
}
