using Domain.Framework.Constants;
using FluentMigrator;
using Migrations.Framework;
namespace Migrations
{
    [Migration(201909062114)]
    public class _201909062114_adding_hireType_tbl : Migration
    {
        public override void Down()
        {
            Delete.Table(TableConstants.HireType);
        }

        public override void Up()
        {
            Create.Table(TableConstants.HireType)
                .WithCommonColumns()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Description").AsString().NotNullable()
                .WithColumn("PaysMoney").AsBoolean().NotNullable();
        }
    }
}
