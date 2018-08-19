using System;
using System.Linq;
using AppService.Framework;
using Data.Repositories;
using Domain;

namespace AppService.Services
{
    public class UserService : BaseService, IUserService
    {
        readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User GetById(int userId)
        {
            return _userRepository.GetById(userId);
        }

        public User GetByEmail(string email)
        {
            return _userRepository.Get(x=>x.IsActive && x.Email == email).FirstOrDefault();
        }

        public TaskResult ValidateOnCreate(User entity)
        {
            if (_userRepository.Get(x => x.IsActive && x.Email == entity.Email).Count() > 0)
                TaskResult.AddErrorMessage("El correo del usuario que intentas crear ya existe");
            
            return TaskResult;
        }

        public TaskResult Create(User entity)
        {
            ValidateOnCreate(entity);
            try
            {
                _userRepository.Insert(entity);
                _userRepository.CommitChanges();
            }
            catch (Exception ex)
            {
                TaskResult.Exception = ex;
                TaskResult.AddErrorMessage(ex.Message);
            }
            return TaskResult;
        }

        public TaskResult Delete(int entityId)
        {
            throw new NotImplementedException();
        }


        public TaskResult Update(User entity)
        {
            throw new NotImplementedException();
        }

        public TaskResult ValidateOnDelete(User entity)
        {
            throw new NotImplementedException();
        }

        public TaskResult ValidateOnUpdate(User entity)
        {
            throw new NotImplementedException();
        }
    }

    public interface IUserService : IMutableService<User>
    {
        User GetById(int userId);
        User GetByEmail(string email);
    }
}
