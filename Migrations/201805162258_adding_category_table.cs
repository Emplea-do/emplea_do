using System;
using Domain.Framework.Constants;
using FluentMigrator;
using FluentMigrator.SqlServer;
using Migrations.Framework;

namespace Migrations
{
    [Migration(201805162258)]
    public class _201805162258_adding_category_table : Migration
    {
        public override void Down()
        {
            Delete.Table(TableName.Category);
        }

        public override void Up()
        {
            Create.Table(TableName.Category)
                  .WithColumn("Id").AsInt32().PrimaryKey()
                  .WithColumn("Name").AsString().NotNullable()
                  .WithColumn("Description").AsString().NotNullable()
                  .WithColumn("CreatedAt").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                  .WithColumn("DeletedAt").AsDateTime().Nullable()
                  .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

            Insert.IntoTable(TableName.Category).WithIdentityInsert()
                  .Row(new { Id = CategoryConstants.None, Name = "N/A", Description = "", IsActive = true, CreatedAt = DateTime.UtcNow })
                  .Row(new { Id = CategoryConstants.GraphicDesign, Name = "Diseño Gráfico", Description = "", IsActive = true, CreatedAt = DateTime.UtcNow })
                  .Row(new { Id = CategoryConstants.WebDevelopment, Name = "Desarrollo Web", Description = "", IsActive = true, CreatedAt = DateTime.UtcNow })
                  .Row(new { Id = CategoryConstants.MobileDevelopment, Name = "Desarrollo Móvil", Description = "", IsActive = true, CreatedAt = DateTime.UtcNow })
                  .Row(new { Id = CategoryConstants.SoftwareDevelopment, Name = "Desarrollo de software", Description = "", IsActive = true, CreatedAt = DateTime.UtcNow })
                  .Row(new { Id = CategoryConstants.SystemAdministrator, Name = "Administración de sistemas", Description = "", IsActive = true, CreatedAt = DateTime.UtcNow })
                  .Row(new { Id = CategoryConstants.Networking, Name = "Seguridad de Redes y telecomunicaciones", Description = "", IsActive = true, CreatedAt = DateTime.UtcNow })
                  .Row(new { Id = CategoryConstants.ItSales, Name = "Ventas de IT", Description = "", IsActive = true, CreatedAt = DateTime.UtcNow })
                  .Row(new { Id = CategoryConstants.DataBaseAdministrator, Name = "Administración de base de datos", Description = "", IsActive = true, CreatedAt = DateTime.UtcNow })
                  .Row(new { Id = CategoryConstants.GameDevelopment, Name = "Desarrollo de Juegos", Description = "", IsActive = true, CreatedAt = DateTime.UtcNow })
                  .Row(new { Id = CategoryConstants.TechSupport, Name = "Soporte Técnico", Description = "", IsActive = true, CreatedAt = DateTime.UtcNow });

        }
    }
}
