using System;
using AppServices.Framework;
using Data.Repositories;
using Domain.Entities;

namespace AppServices.Services
{
    public class LoginsService : BaseService<Login, ILoginsRepository>, ILoginsService
    {
        public LoginsService(ILoginsRepository mainRepository) : base(mainRepository)
        {
        }

        protected override TaskResult<Login> ValidateOnCreate(Login entity)
        {
            return new TaskResult<Login>();
        }

        protected override TaskResult<Login> ValidateOnDelete(Login entity)
        {
            return new TaskResult<Login>();
        }

        protected override TaskResult<Login> ValidateOnUpdate(Login entity)
        {
            return new TaskResult<Login>();
        }
    }

    public interface ILoginsService : IBaseService<Login, ILoginsRepository>
    {

    }
}
