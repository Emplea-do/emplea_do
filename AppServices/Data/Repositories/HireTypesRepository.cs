using System;
using Domain.Entities;

namespace AppServices.Data.Repositories
{
    public class HireTypesRepository : BaseRepository<HireType>, IHireTypesRepository
    {
        public HireTypesRepository(EmpleaDbContext database) : base(database)
        {
        }
    }

    public interface IHireTypesRepository : IBaseRepository<HireType> { }
}
