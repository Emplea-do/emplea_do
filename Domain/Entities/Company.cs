using System;
namespace Domain.Entities

{
    public class Company : Entity
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string Email { get; set; }

        public string LogoUrl { get; set; }

        public int? UserId { get; set; }
    }
}
