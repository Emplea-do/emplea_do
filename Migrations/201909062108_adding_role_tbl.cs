using Domain.Framework.Constants;
using FluentMigrator;
using Migrations.Framework;

namespace Migrations
{

    [Migration(20190906108)]
    public class _20190906108_adding_role_tbl : Migration
    {
        public override void Down()
        {
            Delete.Table(TableConstants.Role);
        }

        public override void Up()
        {
            Create.Table(TableConstants.Role)
                .WithCommonColumns()
                .WithColumn("Name").AsString().NotNullable();
        }
    }
}



