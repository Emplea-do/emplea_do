using System;
using System.Collections.Generic;
using Domain;

namespace Data.Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(EmpleadoDbContext database) : base(database)
        {
        }
    }
    public interface IRoleRepository : IBaseRepository<Role>
    {

    }
}
