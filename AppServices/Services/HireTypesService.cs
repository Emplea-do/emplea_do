using System;
using AppServices.Framework;
using AppServices.Data.Repositories;
using Domain.Entities;

namespace AppServices.Services
{
    public class HireTypesService : BaseService<HireType, IHireTypesRepository>, IHireTypesService
    {
        public HireTypesService(IHireTypesRepository mainRepository) : base(mainRepository)
        {
        }

        protected override TaskResult<HireType> ValidateOnCreate(HireType entity)
        {
            return new TaskResult<HireType>();
        }

        protected override TaskResult<HireType> ValidateOnDelete(HireType entity)
        {
            return new TaskResult<HireType>();
        }

        protected override TaskResult<HireType> ValidateOnUpdate(HireType entity)
        {
            return new TaskResult<HireType>();
        }
    }

    public interface IHireTypesService : IBaseService<HireType, IHireTypesRepository>
    {

    }
}
