
namespace CoachSeek.WebUI.Models
{
    public class Location
    {
        public Identifier Identifier { get; set; }
        public string Name { get; set; }


        public Location()
        {
            Identifier = new Identifier();
        }

        public Location(Identifier identifier)
        {
            Identifier = identifier;
        }
    }
}