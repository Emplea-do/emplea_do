using Domain.Framework.Constants;
using FluentMigrator;
using Migrations.Framework;
namespace Migrations
{
    [Migration(201909062110)]
    public class _201909062110_adding_login_tbl : Migration
    {
        public override void Down()
        {
            Delete.Table(TableConstants.Login);
        }

        public override void Up()
        {
            Create.Table(TableConstants.Login)
                .WithCommonColumns()
                .WithColumn("UserId").AsInt32().NotNullable()
                .WithColumn("LoginProvider").AsString().NotNullable()
                .WithColumn("ProviderKey").AsString().NotNullable();
        }
    }
}

