﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
//using System.Data.Entity.ModelConfiguration;
using Domain;
//using System.Data.Entity.ModelConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.TableConfigurations
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
