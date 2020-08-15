using System;
using Domain.Entities;
using Domain.Framework.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppServices.Data.TableConfigurations
{
    public class LocationsTableConfiguration : TableConfiguration<Location>
    {
        public override void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.ToTable(TableConstants.Locations);
        }
    }
}
