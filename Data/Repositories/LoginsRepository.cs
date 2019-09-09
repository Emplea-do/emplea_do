using System;
using Domain.Entities;

namespace Data.Repositories
{
    public class LoginsRepository : BaseRepository<Login>, ILoginsRepository
    {
        public LoginsRepository(EmpleaDbContext database) : base(database)
        {
        }
    }

    public interface ILoginsRepository : IBaseRepository<Login> { }
}
