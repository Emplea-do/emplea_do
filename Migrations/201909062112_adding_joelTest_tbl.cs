using Domain.Framework.Constants;
using FluentMigrator;
using Migrations.Framework;

namespace Migrations
{
    [Migration(201909062112)]
    public class _201909062112_adding_joelTest_tbl : Migration
    {
        public override void Down()
        {
            Delete.Table(TableConstants.joelTest);
        }

        public override void Up()
        {
            Create.Table(TableConstants.joelTest)
                .WithCommonColumns()
                .WithColumn("HasSourceControl").AsBoolean().NotNullable()
                 .WithColumn("HasOneStepBuilds").AsBoolean().NotNullable()
                  .WithColumn("HasDailyBuilds").AsBoolean().NotNullable()
                   .WithColumn("HasBugDatabase").AsBoolean().NotNullable()
                    .WithColumn("HasBusFixedBeforeProceding").AsBoolean().NotNullable()
                     .WithColumn("HasUpToDateSchedule").AsBoolean().NotNullable()
                      .WithColumn("HasSpec").AsBoolean().NotNullable()
                       .WithColumn("HasQuiteEnvironment").AsBoolean().NotNullable()
                        .WithColumn("HasBestTools").AsBoolean().NotNullable()
                         .WithColumn("HasTesters").AsBoolean().NotNullable()
                          .WithColumn("HasWrittenTest").AsBoolean().NotNullable()
                          .WithColumn("HasHallwayTests").AsBoolean().NotNullable();
        }
    }
}
