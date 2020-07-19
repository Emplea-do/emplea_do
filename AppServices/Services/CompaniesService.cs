using System;
using System.Collections.Generic;
using System.Linq;
using AppServices.Framework;
using AppServices.Services;
using AppServices.Data.Repositories;
using Domain.Entities;

namespace AppServices.Services
{
    public class CompaniesService : BaseService<Company, ICompaniesRepository>, ICompaniesService
    {
        public CompaniesService(ICompaniesRepository mainRepository) : base(mainRepository)
        {
        }

        public List<Company> GetByUserId(int userId)
        {
            return _mainRepository.Get(x=>x.IsActive && x.UserId == userId).ToList();
        }

         public Company GetById(int id)
        {
            return _mainRepository.Get(x=>x.IsActive && x.Id == id).FirstOrDefault();
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
        List<Company> GetByUserId(int userId);
         Company GetById(int id);
    }
}
