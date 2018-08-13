using System;
using System.Collections.Generic;
using Domain;

namespace Data.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(EmpleadoDbContext database) : base(database)
        {
        }
    }

    public interface IUserRepository : IBaseRepository<User>
    {

    }
}