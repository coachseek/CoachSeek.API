using System;

namespace CoachSeek.WebUI.Models.Persistence
{
    public class DbLocation
    {
        public Guid Id { get; set; }
        public Guid BusinessId { get; set; }

        public string Name { get; set; }
    }
}