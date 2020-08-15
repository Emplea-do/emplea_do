using System;
using Domain.Entities;

namespace AppServices.Data.Repositories
{
    public class JobsRepository : BaseRepository<Job>, IJobsRepository
    {
        public JobsRepository(EmpleaDbContext database) : base(database)
        {
        }
    }

    public interface IJobsRepository : IBaseRepository<Job> { }
}
