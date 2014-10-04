using System;

namespace CoachSeek.WebUI.Models
{
    public class Location
    {
        public Guid Id { get; set; }
        public string Name { get; set; }


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