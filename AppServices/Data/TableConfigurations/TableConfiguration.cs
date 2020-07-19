using System;
using System.ComponentModel.DataAnnotations.Schema;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppServices.Data.TableConfigurations
{
    public abstract class TableConfiguration<T> : IEntityTypeConfiguration<T> where T : Entity
    {
        protected void CommonColumnsConfiguration(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.CreatedAt).IsRequired();
            builder.Property(p => p.IsActive).IsRequired();
        }
        public abstract void Configure(EntityTypeBuilder<T> builder);
    }
}
