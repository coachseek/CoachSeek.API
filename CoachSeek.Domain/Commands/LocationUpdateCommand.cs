using System;
using AutoMapper;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Commands
{
    public class LocationUpdateCommand : IBusinessIdable
    {
        public Guid BusinessId { get; set; }
        public Guid Id { get; set; }

        public string Name { get; set; }


        public LocationData ToData()
        {
            return Mapper.Map<LocationUpdateCommand, LocationData>(this);
        }
    }
}