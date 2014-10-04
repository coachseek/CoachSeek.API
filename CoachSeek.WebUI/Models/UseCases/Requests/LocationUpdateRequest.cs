using System;

namespace CoachSeek.WebUI.Models.UseCases.Requests
{
    public class LocationUpdateRequest
    {
        public Guid BusinessId { get; set; }
        public Guid? LocationId { get; set; }
        public string LocationName { get; set; }
    }
}