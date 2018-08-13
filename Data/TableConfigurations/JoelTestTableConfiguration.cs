using System;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.TableConfigurations
{
    public class JoelTestTableConfiguration: TableConfiguration<JoelTest>
    {
        public override void Configure(EntityTypeBuilder<JoelTest> builder)
        {
            builder.ToTable("JoelTests");

            CommonColumnsConfiguration(builder);
        }
    }
}
