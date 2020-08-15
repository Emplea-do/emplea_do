using AppServices.Services;
using AppServices.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using Web.Services.Slack;

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

            services.AddSingleton<LegacyApiClient, LegacyApiClient>();    
            //services.AddSingleton<IJobsService, MockJobsService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ISlackService, SlackService>();

        }

    }
}
