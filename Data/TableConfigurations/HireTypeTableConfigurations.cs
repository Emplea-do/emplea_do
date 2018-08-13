using System;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.TableConfigurations
{
    public class HireTypeTableConfigurations : TableConfiguration<HireType>
    {
        public override void Configure(EntityTypeBuilder<HireType> builder)
        {
            builder.ToTable("HireTypes");

            CommonColumnsConfiguration(builder);
        }
    }
}
