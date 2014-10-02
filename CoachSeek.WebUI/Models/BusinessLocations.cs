using System.Collections.Generic;
using System.Linq;
using CoachSeek.WebUI.Exceptions;

namespace CoachSeek.WebUI.Models
{
    public class BusinessLocations
    {
        public List<Location> Locations { get; private set; }

        public BusinessLocations()
        {
            Locations = new List<Location>();
        }

        public void Add(Location location)
        {
            Validate(location);
            Locations.Add(location);
        }

        private void Validate(Location location)
        {
            var isExistingLocation = Locations.Any(x => x.Name.ToLower() == location.Name.ToLower());
            if (isExistingLocation)
                throw new DuplicateLocationException();
        }
    }
}