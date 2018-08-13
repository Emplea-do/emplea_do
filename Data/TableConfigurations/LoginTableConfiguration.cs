using System;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.TableConfigurations
{
    public class LoginTableConfiguration : TableConfiguration<Login>
    {
        public override void Configure(EntityTypeBuilder<Login> builder)
        {
            builder.ToTable("Logins");

            CommonColumnsConfiguration(builder);
        }
    }
}
