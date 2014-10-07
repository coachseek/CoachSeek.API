using System;

namespace CoachSeek.WebUI.Models
{
    public class Coach
    {
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _phone;

        public Guid Id { get; set; }

        public string FirstName 
        {
            get { return _firstName; }
            set { _firstName = value.Trim(); }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value.Trim(); }
        }

        public string Email 
        {
            get { return _email; }
            set { _email = value.Trim().ToLower(); }
        }

        public string Phone
        {
            get { return _phone; }
            set { _phone = value.Trim(); }
        }

        public string Name
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }

        public Coach()
        {
            Id = Guid.NewGuid();
        }

        public Coach(Guid id)
        {
            Id = id;
        }
    }
}