using System;
using System.Collections.Generic;
using Domain;

namespace Data.Repositories
{
    public class LocationRepository : BaseRepository<Location>, ILocationRepository
    {
        public LocationRepository(EmpleadoDbContext database) : base(database)
        {
        }
    }

    public interface ILocationRepository : IBaseRepository<Location>
    {
        
    }
}
