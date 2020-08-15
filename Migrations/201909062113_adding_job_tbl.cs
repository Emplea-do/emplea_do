using Domain.Framework.Constants;
using FluentMigrator;
using Migrations.Framework;

namespace Migrations
{
    [Migration(201909062113)]
    public class _201909062113_adding_job_tbl : Migration
    {
        public override void Down()
        {
            Delete.Table(TableConstants.Jobs);
        }

        public override void Up()
        {
            Create.Table(TableConstants.Jobs)
                .WithCommonColumns()
                .WithColumn("UserId").AsInt32().Nullable()
                .WithColumn("Title").AsString(int.MaxValue).NotNullable()
                .WithColumn("Description").AsString(int.MaxValue).NotNullable()
                .WithColumn("HowToApply").AsString(int.MaxValue).NotNullable()
                .WithColumn("ViewCount").AsInt32().NotNullable()
                .WithColumn("Likes").AsInt32().NotNullable()
                .WithColumn("IsRemote").AsBoolean().NotNullable()
                .WithColumn("IsHidden").AsBoolean().NotNullable()
                .WithColumn("IsApproved").AsBoolean().NotNullable()
                .WithColumn("PublishedDate").AsDateTime().NotNullable()
                .WithColumn("CategoryId").AsInt32().NotNullable()
                .WithColumn("CompanyId").AsInt32().Nullable()
                .WithColumn("HireTypeId").AsInt32().NotNullable()
                .WithColumn("JoelTestId").AsInt32().Nullable()
                .WithColumn("LocationId").AsInt32().Nullable();

        }
    }
}

