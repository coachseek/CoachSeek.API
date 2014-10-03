using System.Collections.Generic;
using CoachSeek.WebUI.Contracts.Persistence;

namespace CoachSeek.WebUI.Models
{
    public class Business
    {
        public Identifier Identifier { get; set; }
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
            Identifier = new Identifier();
            BusinessLocations = new BusinessLocations();
        }

        public Business(Identifier identifier) 
            : this()
        {
            Identifier = identifier;
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
    }
}