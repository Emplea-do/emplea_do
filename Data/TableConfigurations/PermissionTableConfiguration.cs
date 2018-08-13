using System;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.TableConfigurations
{
    public class PermissionTableConfiguration : TableConfiguration<Permission>
    {
        public override void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permissions");

            CommonColumnsConfiguration(builder);
        }
    }
}
