using System;
using AutoMapper;
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
            Name = name != null ? name.Trim() : string.Empty;
        }

        public Location(LocationData data)
            : this(data.Id, data.Name)
        { }


        public LocationData ToData()
        {
            return Mapper.Map<Location, LocationData>(this);
        }

        public LocationKeyData ToKeyData()
        {
            return Mapper.Map<Location, LocationKeyData>(this);
        }
    }
}