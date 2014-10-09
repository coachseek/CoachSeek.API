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
            ValidateAdd(location);
            Locations.Add(location);
        }

        public void Update(Location location)
        {
            ValidateUpdate(location);

            var updateLocation = Locations.Single(x => x.Id == location.Id);
            var updateIndex = Locations.IndexOf(updateLocation);
            Locations[updateIndex] = location;
        }

        private void ValidateAdd(Location location)
        {
            var isExistingLocation = Locations.Any(x => x.Name.ToLower() == location.Name.ToLower());
            if (isExistingLocation)
                throw new DuplicateLocation();
        }

        private void ValidateUpdate(Location location)
        {
            var isExistingLocation = Locations.Any(x => x.Id == location.Id);
            if (!isExistingLocation)
                throw new InvalidLocation();

            var existingLocation = Locations.FirstOrDefault(x => x.Name.ToLower() == location.Name.ToLower());
            if (existingLocation != null && existingLocation.Id != location.Id)
                throw new DuplicateLocation();
        }
    }
}