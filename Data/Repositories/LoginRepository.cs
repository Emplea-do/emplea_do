using System;
using System.Collections.Generic;
using Domain;

namespace Data.Repositories
{
    public class LoginRepository : BaseRepository<Login>, ILoginRepository
    {
        public LoginRepository(EmpleadoDbContext database) : base(database)
        {
        }
    }

    public interface ILoginRepository : IBaseRepository<Login>
    {
    }
}
