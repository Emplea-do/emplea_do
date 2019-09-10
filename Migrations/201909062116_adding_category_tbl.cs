using Domain.Framework.Constants;
using FluentMigrator;
using Migrations.Framework;

namespace Migrations
{
    [Migration(201909062116)]
    public class _201909062116_adding_category_tbl : Migration
    {
        public override void Down()
        {
            Delete.Table(TableConstants.Categories);
        }

        public override void Up()
        {
            Create.Table(TableConstants.Categories)
                .WithCommonColumns()
                .WithColumn("Name").AsInt32().NotNullable()
                .WithColumn("Description").AsString().NotNullable();
        }
    }
}
