using System;
using System.Collections.Generic;
using Domain;

namespace Data.Repositories
{
    public class PermissionRepository : BaseRepository<Permission>, IPermissionRepository
    {
        protected PermissionRepository(EmpleadoDbContext database) : base(database)
        {
        }
    }
    public interface IPermissionRepository : IBaseRepository<Permission>
    {

    }
}
