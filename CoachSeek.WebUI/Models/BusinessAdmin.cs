namespace CoachSeek.WebUI.Models
{
    public class BusinessAdmin
    {
        public int Id { get; set; }
        public int BusinessId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}