using System;
using System.Collections.Generic;
using Domain;
using Domain.Framework.Constants;
namespace Data.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {        
        public CategoryRepository(EmpleadoDbContext database) : base(database)
        {
        }
    }

    public interface ICategoryRepository : IBaseRepository<Category>
    {
    }
}
