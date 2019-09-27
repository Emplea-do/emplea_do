using System;
using Domain.Entities;

namespace Data.Repositories
{
    public class CategoriesRepository : BaseRepository<Category>, ICategoriesRepository
    {
        public CategoriesRepository(EmpleaDbContext database) : base(database)
        {
        }
    }

    public interface ICategoriesRepository : IBaseRepository<Category> { }
}
