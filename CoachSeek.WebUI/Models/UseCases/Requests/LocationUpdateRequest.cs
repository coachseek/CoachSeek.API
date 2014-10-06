using System;

namespace CoachSeek.WebUI.Models.UseCases.Requests
{
    public class LocationUpdateRequest : Request
    {
        public Guid LocationId { get; set; }
        public string LocationName { get; set; }
    }
}