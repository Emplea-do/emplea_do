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
using Microsoft.Extensions.Hosting;

namespace Web
{
    public class Program
    {
        public static IWebHostEnvironment HostingEnvironment;
        public static void Main(string[] args)
        {
            try
            {
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (System.ArgumentException ex)
            {
                Console.Error.WriteLine(ex);
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
           .ConfigureAppConfiguration((hostingContext, config) =>
           {
               HostingEnvironment = hostingContext.HostingEnvironment;
               ConfigureAzureAppConfiguration(ref config);
           }).UseStartup<Startup>();

        private static void ConfigureAzureAppConfiguration(ref IConfigurationBuilder config)
        {
            // For the production environment, we are going to use 
            //Azure App Configuration as our provider of (1) App Configuration and (2) Feature Flags

            if (HostingEnvironment.IsProduction())
            {

                var settings = config.Build();

                var connectionString = settings["AzureAppConfigurationConnectionString"];
                config.AddAzureAppConfiguration(options =>
                {
                    options.Connect(connectionString)
                    .UseFeatureFlags(); // Very Important. It wires up the FeatureManagement capabilities to Azure App Configuration.
                });
            }
        }
    }
}
