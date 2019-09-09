using System;
using Domain.Framework.Constants;
using FluentMigrator;
using FluentMigrator.SqlServer;

namespace Migrations
{
    [Migration(201909090134)]
    public class _201909090134_inserting_dummy_job_data : Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            //  Insert.IntoTable(TableConstants.Jobs).WithIdentityInsert();
            //  Insert.IntoTable(TableConstants.Jobs).WithIdentityInsert();
            //Insert.IntoTable(TableConstants.Jobs).WithIdentityInsert();
        }
    }
}