using System;
using System.Collections.Generic;
using Data.Repositories;
using Domain;
using Domain.Framework.Constants;

namespace Data.Repositories
{
    public class HireTypeRepository : BaseRepository<HireType>, IHireTypeRepository
    {        
        public HireTypeRepository(EmpleadoDbContext database) : base(database)
        {
        }
    }

    public interface IHireTypeRepository : IBaseRepository<HireType>
    {
    }
}
