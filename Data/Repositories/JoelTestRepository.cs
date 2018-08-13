using System;
using System.Collections.Generic;
using Domain;

namespace Data.Repositories
{
    public class JoelTestRepository : BaseRepository<JoelTest>, IJoelTestRepository
    {
        public JoelTestRepository(EmpleadoDbContext database) : base(database)
        {
        }
    }

    public interface IJoelTestRepository : IBaseRepository<JoelTest>
    {
    }
}
