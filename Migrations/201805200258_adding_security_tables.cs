using System;
using FluentMigrator;
using Migrations.Framework;

namespace Migrations
{
    [Migration(201805200258)]
    public class _201805200258_adding_security_tables : Migration
    {
        public override void Down()
        {
            Delete.ForeignKey().FromTable("RolePermissions").ForeignColumn("PermissionId").ToTable("Permissions");
            Delete.ForeignKey().FromTable("RolePermissions").ForeignColumn("RoleId").ToTable("Roles");
            Delete.ForeignKey().FromTable("Logins").ForeignColumn("UserId").ToTable("Users");
            Delete.Table(TableName.Login);
            Delete.Table(TableName.User);
        }

        public override void Up()
        {
            Create.Table(TableName.User)
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                  .WithColumn("LegacyId").AsInt32().Nullable()
                  .WithColumn("Email").AsString().Nullable()
                  .WithColumn("PasswordHash").AsString().Nullable()
                  .WithColumn("Salt").AsString().NotNullable()
                  .WithColumn("CreatedAt").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                  .WithColumn("DeletedAt").AsDateTime().Nullable()
                  .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

            Create.Table(TableName.Login)
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                  .WithColumn("UserId").AsInt32().NotNullable().ForeignKey(TableName.User,"Id")
                  .WithColumn("LoginProvider").AsString().NotNullable()
                  .WithColumn("ProviderKey").AsString().NotNullable()
                  .WithColumn("CreatedAt").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                  .WithColumn("DeletedAt").AsDateTime().Nullable()
                  .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

            Create.Table(TableName.UserRoleJoin)
                  .WithColumn("RoleId").AsInt32().NotNullable().ForeignKey(TableName.Role, "Id").PrimaryKey()
                  .WithColumn("UserId").AsInt32().NotNullable().ForeignKey(TableName.User, "Id").PrimaryKey();


            Create.Table(TableName.Role)
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                  .WithColumn("Name").AsString().NotNullable()
                  .WithColumn("CreatedAt").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                  .WithColumn("DeletedAt").AsDateTime().Nullable()
                  .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);
            
            Create.Table(TableName.Permission)
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                  .WithColumn("Name").AsString().Nullable()
                  .WithColumn("CreatedAt").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                  .WithColumn("DeletedAt").AsDateTime().Nullable()
                  .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

            Create.Table(TableName.RolePermissionJoin)
                  .WithColumn("RoleId").AsInt32().NotNullable().ForeignKey(TableName.Role, "Id").PrimaryKey()
                  .WithColumn("PermissionId").AsInt32().NotNullable().ForeignKey(TableName.Permission, "Id").PrimaryKey();
        }
    }
}
