using System;
using Domain.Entities;
using Domain.Framework.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppServices.Data.TableConfigurations
{
    public class UsersTableConfiguration : TableConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(TableConstants.Users);
        }
    }
}
