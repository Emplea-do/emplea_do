using Domain.Framework.Constants;
using FluentMigrator;

namespace Migrations
{
    [Migration(202211221230)]
    public class _202211221230_alter_companies_url_logoUrl_as_nullable : Migration
    {
        public override void Up()
        {
            Alter.Table(TableConstants.Companies)
                .AlterColumn("Url")
                .AsString()
                .Nullable();

            Alter.Table(TableConstants.Companies)
                .AlterColumn("LogoUrl")
                .AsString()
                .Nullable();
        }

        public override void Down()
        {
            Alter.Table(TableConstants.Companies)
                .AlterColumn("Url")
                .AsString()
                .NotNullable();

            Alter.Table(TableConstants.Companies)
                .AlterColumn("LogoUrl")
                .AsString()
                .NotNullable();
        }
    }
}