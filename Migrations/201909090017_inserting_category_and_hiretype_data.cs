using System;
using Domain.Framework.Constants;
using FluentMigrator;
using FluentMigrator.SqlServer;

namespace Migrations
{
    [Migration(201909090017)]
    public class _201909090017_inserting_category_and_hiretype_data : Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            Insert.IntoTable(TableConstants.Categories).WithIdentityInsert()
                .Row(new { id = 1, Name = "Diseño Gráfico", Description = "", IsActive = true, CreatedAt = DateTime.UtcNow })
                .Row(new { id = 2, Name = "Desarrollo Web", Description = "", IsActive = true, CreatedAt = DateTime.UtcNow })
                .Row(new { id = 3, Name = "Desarrollo para Móviles", Description = "", IsActive = true, CreatedAt = DateTime.UtcNow })
                .Row(new { id = 4, Name = "Desarrollo de Software", Description = "", IsActive = true, CreatedAt = DateTime.UtcNow })
                .Row(new { id = 5, Name = "Administrador de sistemas", Description = "", IsActive = true, CreatedAt = DateTime.UtcNow })
                .Row(new { id = 6, Name = "Redes y telecomunicaciones", Description = "", IsActive = true, CreatedAt = DateTime.UtcNow })
                .Row(new { id = 7, Name = "IT ventas", Description = "", IsActive = true, CreatedAt = DateTime.UtcNow })
                .Row(new { id = 8, Name = "Administrador de base de datos", Description = "", IsActive = true, CreatedAt = DateTime.UtcNow })
                .Row(new { id = 9, Name = "Desarrollador de videojuegos", Description = "", IsActive = true, CreatedAt = DateTime.UtcNow });

            Insert.IntoTable(TableConstants.HireTypes).WithIdentityInsert()
                .Row(new { id = 1, Name = "Independiente", PaysMoney = true, Description = "", IsActive = true, CreatedAt = DateTime.UtcNow })
                .Row(new { id = 2, Name = "Medio tiempo", PaysMoney = true, Description = "", IsActive = true, CreatedAt = DateTime.UtcNow })
                .Row(new { id = 3, Name = "Tiempo Completo", PaysMoney = true, Description = "", IsActive = true, CreatedAt = DateTime.UtcNow })
                .Row(new { id = 4, Name = "Pasantía / Internado", PaysMoney = true, Description = "", IsActive = true, CreatedAt = DateTime.UtcNow })
                .Row(new { id = 5, Name = "Voluntario", PaysMoney = false, Description = "", IsActive = true, CreatedAt = DateTime.UtcNow });
        }
    }
}
