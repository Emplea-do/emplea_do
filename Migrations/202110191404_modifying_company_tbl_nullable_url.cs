using System;
using Domain.Framework.Constants;
using FluentMigrator;
using FluentMigrator.SqlServer;
using Migrations.Framework;

[Migration(202110191404)]
public class _202110191404_modifying_company_tbl_nullable_url : Migration
{
    public override void Down()
    {
        Alter.Column("Url").OnTable(TableConstants.Companies)
            .AsString()
            .NotNullable();
    }

    public override void Up()
    {
        
#if DEBUG 
        // There's no ALTER COLUMN in sqlite
        //TODO: remove dummy data from migration
        Delete.Table(TableConstants.Companies);
        Create.Table(TableConstants.Companies)
            .WithCommonColumns()
            .WithColumn("Name").AsString().NotNullable()
            .WithColumn("Url").AsString().Nullable()
            .WithColumn("Email").AsString().NotNullable()
            .WithColumn("LogoUrl").AsString().NotNullable()
            .WithColumn("UserId").AsInt32().Nullable();

        Insert.IntoTable(TableConstants.Companies)
            .WithIdentityInsert()
            .Row(new
            {
                Id = -1,
                Name = "Megsoft",
                CreatedAt = DateTime.UtcNow,
                Email = "claudio@megsoftconsulting.com",
                Url = "https://megsoftconsulting.com",
                LogoUrl = "https://megsoftconsulting.com/wp-content/uploads/2018/08/my_business.png",
                UserId = -1
            });
#else
        Alter.Column("Url").OnTable(TableConstants.Companies)
            .AsString()
            .Nullable();
#endif
    }
}