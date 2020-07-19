using System;
using Domain.Entities;
using Domain.Framework.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppServices.Data.TableConfigurations
{
    public class HireTypeTableConfiguration : TableConfiguration<HireType>
    {
        public override void Configure(EntityTypeBuilder<HireType> builder)
        {
            builder.ToTable(TableConstants.HireTypes);
        }
    }
}
