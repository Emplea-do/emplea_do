using System;
using Domain.Entities;
using Domain.Framework.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppServices.Data.TableConfigurations
{
    public class BannersTableConfiguration : TableConfiguration<Banner>
    {
        public override void Configure(EntityTypeBuilder<Banner> builder)
        {
            builder.ToTable(TableConstants.Banners);
        }
    }
}
