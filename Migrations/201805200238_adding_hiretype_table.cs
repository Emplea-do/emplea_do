using System;
using Domain.Framework.Constants;
using FluentMigrator;
using FluentMigrator.SqlServer;
using Migrations.Framework;

namespace Migrations
{
    [Migration(201805200238)]
    public class _201805200238_adding_hiretype_table : Migration
    {        
        public override void Down()
        {
            Delete.Table(TableName.HireType);
        }

        public override void Up()
        {
            Create.Table(TableName.HireType)
                .WithColumn("Id").AsInt32().PrimaryKey()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Description").AsString().NotNullable()
                .WithColumn("PaysMoney").AsBoolean().NotNullable()
                .WithColumn("CreatedAt").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("DeletedAt").AsDateTime().Nullable()
                .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

            Insert.IntoTable(TableName.HireType).WithIdentityInsert()
                .Row(new { Id = HireTypeConstants.NotApplicable, Name = "N/A", PaysMoney=true, Description = "", IsActive = true, CreatedAt = DateTime.UtcNow })
                .Row(new { Id = HireTypeConstants.Freelance, Name = "Freelance", PaysMoney=true, Description = "", IsActive = true, CreatedAt = DateTime.UtcNow })
                .Row(new { Id = HireTypeConstants.PartTime, Name = "Prescencial Medio Tiempo", PaysMoney=true, Description = "", IsActive = true, CreatedAt = DateTime.UtcNow })
                .Row(new { Id = HireTypeConstants.FullTime, Name = "Prescencial Tiempo completo", PaysMoney=true, Description = "", IsActive = true, CreatedAt = DateTime.UtcNow })
                .Row(new { Id = HireTypeConstants.PaidInternship, Name = "Pasantía pagada", PaysMoney=true, Description = "", IsActive = true, CreatedAt = DateTime.UtcNow })
                .Row(new { Id = HireTypeConstants.Volunteer, Name = "Voluntariado", PaysMoney=false, Description = "", IsActive = true, CreatedAt = DateTime.UtcNow })
                .Row(new { Id = HireTypeConstants.FreeInternship, Name = "Pasantía no pagada", PaysMoney=false, Description = "", IsActive = true, CreatedAt = DateTime.UtcNow });
        }
    }
}