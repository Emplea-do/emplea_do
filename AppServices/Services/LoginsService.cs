using System;
using System.Linq;
using AppServices.Framework;
using AppServices.Data.Repositories;
using Domain.Entities;

namespace AppServices.Services
{
    public class LoginsService : BaseService<Login, ILoginsRepository>, ILoginsService
    {
        public LoginsService(ILoginsRepository mainRepository) : base(mainRepository)
        {
        }

        public Login GetLogin(string provider, string socialId)
        {
            return _mainRepository.Get(x=>x.ProviderKey== socialId && x.LoginProvider == provider).FirstOrDefault();
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
        Login GetLogin(string provider, string socialId);
    }
}
