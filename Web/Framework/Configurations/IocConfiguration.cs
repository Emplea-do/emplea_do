using System;
using AppService.Services;
using Data;
using Data.Repositories;
using Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Framework.Configurations
{
    public static class IocConfiguration
    {
        public static void Init(IServiceCollection services)
        {
            services.AddTransient<EmpleadoDbContext, EmpleadoDbContext>();

            // Repositories
            services.AddTransient(typeof(ICategoryRepository), typeof(CategoryRepository));
            services.AddTransient(typeof(IHireTypeRepository), typeof(HireTypeRepository));
            services.AddTransient(typeof(IJobRepository), typeof(JobRepository));
            services.AddTransient(typeof(IPermissionRepository), typeof(PermissionRepository));
            services.AddTransient(typeof(IRoleRepository), typeof(RoleRepository));
            services.AddTransient(typeof(IUserRepository), typeof(UserRepository));
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<IJoelTestRepository, JoelTestRepository>();
            services.AddTransient<ILocationRepository, LocationRepository>();
            services.AddTransient<ILoginRepository, LoginRepository>();

            // Services
            services.AddTransient(typeof(ICategoryService), typeof(CategoryService));
            services.AddTransient(typeof(IHireTypeService), typeof(HireTypeService));
            services.AddTransient(typeof(IJobService), typeof(JobService));
            services.AddTransient(typeof(ISecurityService), typeof(SecurityService));
            services.AddTransient(typeof(IUserService), typeof(UserService));
        }
    }
}
