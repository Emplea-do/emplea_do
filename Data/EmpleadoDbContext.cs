using System;
using System.Diagnostics;
using System.Text;
using Data.TableConfigurations;
using Domain;
using Microsoft.EntityFrameworkCore;
namespace Data
{

    public class EmpleadoDbContext : DbContext
    {
        public EmpleadoDbContext(DbContextOptions<EmpleadoDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new JobTableConfiguration());
            modelBuilder.ApplyConfiguration(new LocationTableConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryTableConfiguration());
            modelBuilder.ApplyConfiguration(new HireTypeTableConfigurations());
            modelBuilder.ApplyConfiguration(new PermissionTableConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyTableConfiguration());
            modelBuilder.ApplyConfiguration(new JoelTestTableConfiguration());
            modelBuilder.ApplyConfiguration(new LoginTableConfiguration());
            modelBuilder.ApplyConfiguration(new RoleTableConfiguration());
            modelBuilder.ApplyConfiguration(new UserTableConfiguration());
        }

        /*
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb, ex
                );
            }
        }
        */
    }
}
