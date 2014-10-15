﻿using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace CoachSeek.Domain.Entities
{
    public class BusinessLocations
    {
        private List<Location> Locations { get; set; }

        public BusinessLocations()
        {
            Locations = new List<Location>();
        }

        public BusinessLocations(IEnumerable<LocationData> locations) 
            : this()
        {
            if (locations == null)
                return;

            foreach(var location in locations)
                Append(location);
        }

        public void Add(NewLocationData newLocationData)
        {
            var newLocation = new NewLocation(newLocationData);
            ValidateAdd(newLocation);
            Locations.Add(newLocation);
        }

        public void Append(LocationData locationData)
        {
            // Data is not Validated. Eg. It comes from the database.
            Locations.Add(new Location(locationData));
        }

        public void Update(LocationData locationData)
        {
            var location = new Location(locationData);
            ValidateUpdate(location);
            ReplaceLocationInLocations(location);
        }

        public IList<LocationData> ToData()
        {
            return Locations.Select(location => location.ToData()).ToList();
        }


        private void ReplaceLocationInLocations(Location location)
        {
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