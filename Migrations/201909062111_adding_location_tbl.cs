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
            Delete.Table(TableConstants.Locations);
        }

        public override void Up()
        {
            Create.Table(TableConstants.Locations)
                .WithCommonColumns()
                .WithColumn("PlaceId").AsString().NotNullable()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Latitude").AsString().NotNullable()
                .WithColumn("Longitude").AsString().NotNullable();
        }
    }
}
