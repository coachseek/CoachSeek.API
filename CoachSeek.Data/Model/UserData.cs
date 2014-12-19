using System;

namespace CoachSeek.Data.Model
{
    public class UserData
    {
        public Guid Id { get; set; }

        public Guid? BusinessId { get; set; }
        public string BusinessName { get; set; } // Debug

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}
