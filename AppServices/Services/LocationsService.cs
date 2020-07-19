using System;
using AppServices.Framework;
using AppServices.Data.Repositories;
using Domain.Entities;

namespace AppServices.Services
{
    public class LocationsService : BaseService<Location, ILocationsRepository>, ILocationsService
    {
        public LocationsService(ILocationsRepository mainRepository) : base(mainRepository)
        {
        }

        protected override TaskResult<Location> ValidateOnCreate(Location entity)
        {
            return new TaskResult<Location>();
        }

        protected override TaskResult<Location> ValidateOnDelete(Location entity)
        {
            return new TaskResult<Location>();
        }

        protected override TaskResult<Location> ValidateOnUpdate(Location entity)
        {
            return new TaskResult<Location>();
        }
    }

    public interface ILocationsService : IBaseService<Location, ILocationsRepository>
    {

    }
}
