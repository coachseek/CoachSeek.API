using System;
using CoachSeek.Domain.Data;

namespace CoachSeek.Domain.Commands
{
    public class LocationAddRequest : IBusinessIdable
    {
        public Guid BusinessId { get; set; }
        public string LocationName { get; set; }


        public NewLocationData ToData()
        {
            return new NewLocationData
            {
                Name = LocationName
            };
        }
    }
}