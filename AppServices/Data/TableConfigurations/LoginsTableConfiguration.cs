using System;
using Domain.Entities;
using Domain.Framework.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppServices.Data.TableConfigurations
{
    public class LoginsTableConfiguration : TableConfiguration<Login>
    {
        public override void Configure(EntityTypeBuilder<Login> builder)
        {
            builder.ToTable(TableConstants.Logins);
        }
    }
}
