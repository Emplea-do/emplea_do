using System;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.TableConfigurations
{
    public class JobTableConfiguration : TableConfiguration<Job>
    {
        public override void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.ToTable("Jobs");

            CommonColumnsConfiguration(builder);
        }
    }
}
