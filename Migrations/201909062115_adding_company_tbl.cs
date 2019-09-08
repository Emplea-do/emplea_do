using Domain.Framework.Constants;
using FluentMigrator;
using Migrations.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Migrations
{
    [Migration(201909062115)]
    public class _201909062115_adding_company_tbl : Migration
    {
        public override void Down()
        {
            Delete.Table(TableConstants.Company);
        }

        public override void Up()
        {
            Create.Table(TableConstants.Company)
                .WithCommonColumns()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Url").AsString().NotNullable()
                .WithColumn("Email").AsString().NotNullable()
                 .WithColumn("LogoUrl").AsString().NotNullable()
                  .WithColumn("UserId").AsInt32().Nullable();
        }
    }
}
