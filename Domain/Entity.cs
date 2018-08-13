using System;
namespace Domain
{
    public class Entity
    {
        public int Id { get; set; }

        public bool IsActive { get; set; }

        public DateTime? DeletedAt { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
