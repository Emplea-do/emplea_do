using System;
using Domain.Framework.Constants;
using FluentMigrator;
using Migrations.Framework;

namespace Migrations
{
    [Migration(201909022107)]
    public class _201909022107_adding_users_table : Migration
    {
        public override void Down()
        {
            Delete.Table(TableConstants.Users);
        }

        public override void Up()
        {
            Create.Table(TableConstants.Users)
                .WithCommonColumns()
                .WithColumn("Email").AsString().NotNullable();
        }
    }
}
