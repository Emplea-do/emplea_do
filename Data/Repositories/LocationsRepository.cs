using System;
using Domain.Entities;

namespace Data.Repositories
{
    public class LocationsRepository : BaseRepository<Location>, ILocationsRepository
    {
        public LocationsRepository(EmpleaDbContext database) : base(database)
        {
        }
    }

    public interface ILocationsRepository : IBaseRepository<Location> { }
}
