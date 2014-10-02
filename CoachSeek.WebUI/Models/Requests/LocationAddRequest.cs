using System;

namespace CoachSeek.WebUI.Models.Requests
{
    public class LocationAddRequest
    {
        public Guid BusinessId { get; set; }
        public string LocationName { get; set; }
    }
}