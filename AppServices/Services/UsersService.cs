using System;
using AppServices.Framework;
using AppServices.Data.Repositories;
using Domain.Entities;

namespace AppServices.Services
{
    public class UsersService : BaseService<User, IUsersRepository>, IUsersService
    {
        public UsersService(IUsersRepository mainRepository) : base(mainRepository)
        {
        }

        protected override TaskResult<User> ValidateOnCreate(User entity)
        {
            return new TaskResult<User>();
        }

        protected override TaskResult<User> ValidateOnDelete(User entity)
        {
            return new TaskResult<User>();
        }

        protected override TaskResult<User> ValidateOnUpdate(User entity)
        {
            return new TaskResult<User>();
        }
    }

    public interface IUsersService : IBaseService<User, IUsersRepository>
    {

    }
}
