using System;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.TableConfigurations
{
    public class CompanyTableConfiguration : TableConfiguration<Company>
    {
        public override void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Companies");

            CommonColumnsConfiguration(builder);
        }
    }
}
