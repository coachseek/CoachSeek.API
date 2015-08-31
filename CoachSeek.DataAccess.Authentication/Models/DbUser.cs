using System;

namespace CoachSeek.DataAccess.Authentication.Models
{
    public class DbUser
    {
        public Guid Id { get; set; }

        public Guid? BusinessId { get; set; }
        public string BusinessName { get; set; } // Debug

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}
