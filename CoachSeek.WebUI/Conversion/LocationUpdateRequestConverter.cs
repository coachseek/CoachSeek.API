using System;
using CoachSeek.Domain.Commands;
using CoachSeek.WebUI.Models.Api;

namespace CoachSeek.WebUI.Conversion
{
    public static class LocationUpdateRequestConverter
    {
        public static LocationUpdateRequest Convert(ApiLocationSaveRequest apiRequest)
        {
            return new LocationUpdateRequest
            {
                LocationId = apiRequest.Id.HasValue ? apiRequest.Id.Value : Guid.Empty,
                BusinessId = apiRequest.BusinessId,
                LocationName = apiRequest.Name
            };
        }
    }
}