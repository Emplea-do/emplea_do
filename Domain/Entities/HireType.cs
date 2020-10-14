using System;
namespace Domain.Entities
{
    public class HireType : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool PaysMoney { get; set; }
    }
}
