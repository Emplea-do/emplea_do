using Domain.Framework.Constants;
using FluentMigrator;

namespace Migrations
{
    [Migration(202007202040)]
    public class _202007202040_add_isenabledforbanners_to_users_table : Migration
    {
        public override void Down()
        {
            Delete.Column("IsEnabledForBanners").FromTable(TableConstants.Users);
        }

        public override void Up()
        {
            Alter.Table(TableConstants.Users).AddColumn("IsEnabledForBanners")
            .AsBoolean().NotNullable().WithDefaultValue(false);
        }
    }
}