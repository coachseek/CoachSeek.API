using System;

namespace CoachSeek.WebUI.Models
{
    public class Location
    {
        private string _name;

        public Guid Id { get; set; }

        public string Name
        {
            get { return _name; }
            set { _name = value.Trim(); }
        }


        public Location()
        {
            Id = Guid.NewGuid();
        }

        public Location(Guid id)
        {
            Id = id;
        }
    }
}