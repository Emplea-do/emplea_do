using System;
using Domain.Entities;

namespace AppServices.Data.Repositories
{
    public class CompaniesRepository : BaseRepository<Company>, ICompaniesRepository
    {
        public CompaniesRepository(EmpleaDbContext database) : base(database)
        {
        }
    }

    public interface ICompaniesRepository : IBaseRepository<Company> { }
}
