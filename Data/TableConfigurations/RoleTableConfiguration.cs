using System;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.TableConfigurations
{
    public class RoleTableConfiguration : TableConfiguration<Role>
    {
        public override void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");

            CommonColumnsConfiguration(builder);
        }
    }
}
