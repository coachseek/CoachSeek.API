using System;
using System.Collections.Generic;
using CoachSeek.WebUI.Contracts.Persistence;

namespace CoachSeek.WebUI.Models
{
    public class Business
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public BusinessAdmin Admin { get; set; }

        private BusinessLocations BusinessLocations { get; set; }


        public IList<Location> Locations
        {
            get { return BusinessLocations.Locations; }
        }

        public Business()
        {
            Id = Guid.NewGuid();
            BusinessLocations = new BusinessLocations();
        }

        public Business(Guid id) 
            : this()
        {
            Id = id;
        }

        public Business(IEnumerable<Location> locations) 
            : this()
        {
            foreach (var location in locations)
                BusinessLocations.Add(location);
        }


        public void AddLocation(Location location, IBusinessRepository businessRepository)
        {
            BusinessLocations.Add(location);
            businessRepository.Save(this);
        }

        public void UpdateLocation(Location location, IBusinessRepository businessRepository)
        {
            BusinessLocations.Update(location);
            businessRepository.Save(this);
        }
    }
}