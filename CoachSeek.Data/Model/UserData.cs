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
        public string Name { get { return string.Format("{0} {1}", FirstName, LastName); } }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }

        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}
