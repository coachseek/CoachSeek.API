using System;

namespace CoachSeek.WebUI.Models.Persistence
{
    public class DbBusinessAdmin
    {
        public Guid Id { get; set; }
        public Guid BusinessId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
    }
}