using Domain.Framework.Constants;
using FluentMigrator;
using Migrations.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Migrations
{
    [Migration(201909062109)]
    public class _201909062109_adding_permission_tbl : Migration
    {
        public override void Down()
        {
            Delete.Table(TableConstants.Permissions);
        }

        public override void Up()
        {
            Create.Table(TableConstants.Permissions)
                .WithCommonColumns()
                .WithColumn("Name").AsString().NotNullable();
        }
    }
}
