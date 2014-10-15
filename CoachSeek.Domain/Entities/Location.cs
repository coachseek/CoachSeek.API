using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Entities
{
    public class Location
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }


        public Location(Guid id, string name)
        {
            Id = id;
            Name = name.Trim();
        }

        public Location(LocationData data)
            : this(data.Id, data.Name)
        { }


        public LocationData ToData()
        {
            return new LocationData
            {
                Id = Id,
                Name = Name
            };
        }
    }
}