using System;
using System.Linq;
using AppService.Framework;
using Data.Repositories;
using Domain;

namespace AppService.Services
{
    public class LoginService : BaseService, ILoginService
    {
        readonly ILoginRepository _loginRepository;

        public LoginService(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        public TaskResult ValidateOnCreate(Login entity)
        {
            if (_loginRepository.Get(x => x.IsActive && x.ProviderKey == entity.ProviderKey && x.LoginProvider == entity.LoginProvider).Count() > 0)
                TaskResult.AddErrorMessage("Este login de usuario ya existe");

            return TaskResult;
        }

        public TaskResult Create(Login entity)
        {
            ValidateOnCreate(entity);
            if(TaskResult.ExecutedSuccesfully)
            {
                try
                {
                    _loginRepository.Insert(entity);
                    _loginRepository.CommitChanges();
                }
                catch(Exception ex)
                {
                    TaskResult.Exception = ex;
                    TaskResult.AddErrorMessage(ex.Message);
                }
            }
            return TaskResult;
        }

        public TaskResult ValidateOnDelete(Login entity)
        {
            throw new NotImplementedException();
        }

        public TaskResult Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public TaskResult ValidateOnUpdate(Login entity)
        {
            throw new NotImplementedException();
        }
        public TaskResult Update(Login entity)
        {
            throw new NotImplementedException();
        }

        public Login Get(string provider, string providerKey)
        {
            return _loginRepository.Get(x => x.IsActive && x.ProviderKey == providerKey && x.LoginProvider == provider).FirstOrDefault();
        }
    }

    public interface ILoginService : IMutableService<Login>
    {
        Login Get(string provider, string providerKey);
    }
}
