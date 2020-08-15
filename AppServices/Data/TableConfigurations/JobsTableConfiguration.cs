using System;
using Domain.Entities;
using Domain.Framework.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppServices.Data.TableConfigurations
{
    public class JobsTableConfiguration : TableConfiguration<Job>
    {
        public override void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.ToTable(TableConstants.Jobs);
        }
    }
}
