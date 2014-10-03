using System;

namespace CoachSeek.WebUI.Models.Requests
{
    public class LocationUpdateRequest
    {
        public Guid BusinessId { get; set; }
        public Guid LocationId { get; set; }
        public string LocationName { get; set; }
    }
}