using Domain.Framework.Constants;
using FluentMigrator;
using Migrations.Framework;

namespace Migrations
{
    [Migration(201909062111)]
    public class _201909062111_adding_location_tbl : Migration
    {
        public override void Down()
        {
            Delete.Table(TableConstants.Location);
        }

        public override void Up()
        {
            Create.Table(TableConstants.Location)
                .WithCommonColumns()
                .WithColumn("PlaceId").AsString().NotNullable()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Latitude").AsDouble().NotNullable()
             .WithColumn("Longitude").AsDouble().NotNullable();

        }
    }
}
