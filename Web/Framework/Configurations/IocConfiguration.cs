using AppServices.Services;
using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
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

        }

    }
}
