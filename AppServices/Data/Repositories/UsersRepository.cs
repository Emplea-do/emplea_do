using System;
using Domain.Entities;

namespace AppServices.Data.Repositories
{
    public class UsersRepository : BaseRepository<User>, IUsersRepository
    {
        public UsersRepository(EmpleaDbContext database) : base(database)
        {
        }
    }

    public interface IUsersRepository: IBaseRepository<User> { }
}
