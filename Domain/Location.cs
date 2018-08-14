using System;
namespace Domain
{
    public class Location : Entity
    {
        public string PlaceId { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
