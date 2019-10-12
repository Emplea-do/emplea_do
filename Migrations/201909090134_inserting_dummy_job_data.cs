using System;
using Domain.Entities;
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
            Insert.IntoTable(TableConstants.Users).WithIdentityInsert()
                .Row(new { Id = -1, Name = "Claudio", Email = "claudio@megsoftconsulting.com" });

            Insert.IntoTable(TableConstants.Companies).WithIdentityInsert()
                .Row(new  { 
                    Id= -1,
                    Name = "Megsoft", 
                    CreatedAt= DateTime.UtcNow, 
                    Email = "claudio@megsoftconsulting.com",
                    Url ="https://megsoftconsulting.com",
                LogoUrl= "https://megsoftconsulting.com/wp-content/uploads/2018/08/my_business.png",
                    UserId =-1});

            Insert.IntoTable(TableConstants.Jobs).WithIdentityInsert()
                .Row(new 
                {
                    Id = -1,
                    Title = "Web Developer wanted",
                    Description = "Yes, you heard",
                    Approved = true,
                    IsHidden = false,
                    IsActive = true,
                    CategoryId = 1,
                    HireTypeId = 1,
                    HowToApply = "just apply",
                    IsRemote = true, 
                    CompanyId= -1,
                    ViewCount = 0,
                    Likes = 0,
                    PublishedDate = DateTime.UtcNow,
                    UserId= -1

                })
                .Row(new
                {
                    Id = -2,
                    Title = "Junior Web Developer wanted",
                    Description = "Yes, you heard",
                    Approved = true,
                    IsHidden = false,
                    IsActive = true,
                    CategoryId = 1,
                    HireTypeId = 1,
                    HowToApply = "just apply",
                    IsRemote = true,
                    CompanyId = -1,
                    ViewCount = 0,
                    Likes = 0,
                    PublishedDate = DateTime.UtcNow,
                    UserId = -1

                }
                );
        }
    }
}