using Domain.Entities;

namespace AppServices.Data.Repositories
{
    public class BannersRepository : BaseRepository<Banner>, IBannersRepository
    {
        public BannersRepository(EmpleaDbContext database) : base(database)
        { }
    }

    public interface IBannersRepository : IBaseRepository<Banner> {}
}