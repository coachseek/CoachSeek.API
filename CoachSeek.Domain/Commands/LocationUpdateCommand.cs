using System;
using AutoMapper;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Commands
{
    public class LocationUpdateCommand : LocationAddCommand, IIdentifiable
    {
        public Guid Id { get; set; }


        public LocationData ToData()
        {
            return Mapper.Map<LocationUpdateCommand, LocationData>(this);
        }
    }
}