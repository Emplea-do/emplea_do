using System;

namespace Domain.Entities
{
    public class Banner : Entity
    {
        public int UserId { get; set; }

        public virtual User User { get; set; }

        public string ImageUrl { get; set; }

        public string DestinationUrl { get; set; }

        public bool IsApproved { get; set; }

        public DateTime ExpirationDay { get; set; } = DateTime.UtcNow.AddDays(30); 
    }
}