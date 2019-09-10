using System;
using Data.TableConfigurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class EmpleaDbContext : DbContext
    {
        public EmpleaDbContext(DbContextOptions<EmpleaDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EmpleaDbContext).Assembly);
        }
    }
}
