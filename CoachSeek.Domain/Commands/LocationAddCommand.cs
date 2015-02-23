using AutoMapper;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Commands
{
    public class LocationAddCommand
    {
        public string Name { get; set; }


        public NewLocationData ToData()
        {
            return Mapper.Map<LocationAddCommand, NewLocationData>(this);
        }
    }
}