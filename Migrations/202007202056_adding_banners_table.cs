using Domain.Framework.Constants;
using FluentMigrator;
using FluentMigrator.Infrastructure;
using Migrations.Framework;

namespace Migrations
{
    [Migration(202007202056)]
    public class _202007202056_adding_banners_table : Migration
    {
        public override void Down()
        {
             Delete.Table(TableConstants.Banners);
        }

        public override void Up()
        {
            Create.Table(TableConstants.Banners)
                .WithCommonColumns()
                .WithColumn("UserId").AsInt32().NotNullable()
                .WithColumn("ImageUrl").AsString().NotNullable()
                .WithColumn("DestinationUrl").AsString().NotNullable()
                .WithColumn("IsApproved").AsBoolean().WithDefaultValue(false)
                .WithColumn("ExpirationDay").AsDateTime().NotNullable();
        }
    }
}