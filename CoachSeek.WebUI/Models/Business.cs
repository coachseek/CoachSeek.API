
namespace CoachSeek.WebUI.Models
{
    public class Business
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public BusinessAdmin Admin { get; set; }
    }
}