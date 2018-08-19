using System;
using AppService.Framework.Social;
using AppService.Services;
using AppService.Services.Social;
using Data;
using Data.Repositories;
using Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Framework.Configurations
{
    public static class IocConfiguration
    {
        public static void Init(IConfiguration configuration, IServiceCollection services)
        {

            var socialKeys = configuration.GetSection("SocialKeys");
            services.Configure<SocialKeys>(socialKeys);

            services.AddTransient<EmpleadoDbContext, EmpleadoDbContext>();

            // Repositories
            services.AddScoped(typeof(ICategoryRepository), typeof(CategoryRepository));
            services.AddScoped(typeof(IHireTypeRepository), typeof(HireTypeRepository));
            services.AddScoped(typeof(IJobRepository), typeof(JobRepository));
            services.AddScoped(typeof(IPermissionRepository), typeof(PermissionRepository));
            services.AddScoped(typeof(IRoleRepository), typeof(RoleRepository));
            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IJoelTestRepository, JoelTestRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<ILoginRepository, LoginRepository>();

            // Services
            services.AddScoped(typeof(ICategoryService), typeof(CategoryService));
            services.AddScoped(typeof(IHireTypeService), typeof(HireTypeService));
            services.AddScoped(typeof(IJobService), typeof(JobService));
            services.AddScoped(typeof(ISecurityService), typeof(SecurityService));
            services.AddScoped(typeof(IUserService), typeof(UserService));
            services.AddScoped<ILoginService, LoginService>();

            services.AddScoped<IFacebookService, FaceBookService>(x => new FaceBookService(socialKeys.GetValue<string>("FacebookAppId"), socialKeys.GetValue<string>("FacebookAppSecret")));
            services.AddScoped<ILinkedinService, LinkedinService>(x => new LinkedinService(socialKeys.GetValue<string>("LinkedInClientId"), socialKeys.GetValue<string>("LinkedInClientSecret")));
            services.AddScoped<IGoogleService, GoogleService>(x => new GoogleService(socialKeys.GetValue<string>("GoogleClientId"), socialKeys.GetValue<string>("GoogleClientSecret")));
            services.AddScoped<IMicrosoftService, MicrosoftService>(x => new MicrosoftService(socialKeys.GetValue<string>("MsClientId"), socialKeys.GetValue<string>("MsClientSecret")));
            services.AddScoped<ISlackService, SlackService>(x => new SlackService(socialKeys.GetValue<string>("slackWebhookEndpoint")));
        }
    }
}
