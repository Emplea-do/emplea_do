using System;
using System.Collections.Generic;
using Domain;

namespace Data.Repositories
{
    public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(EmpleadoDbContext database) : base(database)
        {
        }
    }

    public interface ICompanyRepository : IBaseRepository<Company>
    {
    }
}
