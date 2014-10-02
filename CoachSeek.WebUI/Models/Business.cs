using CoachSeek.WebUI.Contracts.Persistence;

namespace CoachSeek.WebUI.Models
{
    public class Business
    {
        public Identifier Identifier { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public BusinessAdmin Admin { get; set; }
        public BusinessLocations BusinessLocations { get; set; }


        public Business()
        {
            Identifier = new Identifier();
            BusinessLocations = new BusinessLocations();
        }

        public Business(Identifier identifier)
        {
            Identifier = identifier;
            BusinessLocations = new BusinessLocations();
        }


        public void AddLocation(Location location, IBusinessRepository businessRepository)
        {
            BusinessLocations.Add(location);
            businessRepository.Save(this);
        }
    }
}