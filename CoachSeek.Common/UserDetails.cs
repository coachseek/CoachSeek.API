using System;

namespace CoachSeek.Common
{
    public class UserDetails
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }


        public UserDetails(Guid id, string username, string firstName, string lastName)
        {
            Id = id;
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            //Role = "Business Admin";
        }
    }
}
