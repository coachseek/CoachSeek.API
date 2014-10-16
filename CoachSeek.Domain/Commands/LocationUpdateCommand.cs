using System;
using AutoMapper;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Commands
{
    public class LocationUpdateCommand : IBusinessIdable
    {
        public Guid BusinessId { get; set; }
        public Guid LocationId { get; set; }
        public string LocationName { get; set; }


        public LocationData ToData()
        {
            return Mapper.Map<LocationUpdateCommand, LocationData>(this);
        }
    }
}