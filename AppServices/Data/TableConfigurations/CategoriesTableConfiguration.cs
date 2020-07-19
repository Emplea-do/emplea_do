using System;
using Domain.Entities;
using Domain.Framework.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppServices.Data.TableConfigurations
{
    public class CategoriesTableConfiguration : TableConfiguration<Category>
    {
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable(TableConstants.Categories);
        }
    }
}
