using System;
using AppServices.Framework;
using AppServices.Services;
using Data.Repositories;
using Domain.Entities;

namespace AppServices.Services
{
    public class CompaniesService : BaseService<Company, ICompaniesRepository>, ICompaniesService
    {
        public CompaniesService(ICompaniesRepository mainRepository) : base(mainRepository)
        {
        }

        protected override TaskResult<Company> ValidateOnCreate(Company entity)
        {
            return new TaskResult<Company>();
        }

        protected override TaskResult<Company> ValidateOnDelete(Company entity)
        {
            return new TaskResult<Company>();
        }

        protected override TaskResult<Company> ValidateOnUpdate(Company entity)
        {
            return new TaskResult<Company>();
        }
    }

    public interface ICompaniesService : IBaseService<Company, ICompaniesRepository>
    {

    }
}
