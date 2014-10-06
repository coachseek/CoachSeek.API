using System;
using CoachSeek.WebUI.Models.Api;
using CoachSeek.WebUI.Models.UseCases.Requests;

namespace CoachSeek.WebUI.Conversion
{
    public class LocationUpdateRequestConverter
    {
        public static LocationUpdateRequest Convert(ApiLocationSaveRequest request)
        {
            return new LocationUpdateRequest
            {
                BusinessId = request.BusinessId,
                LocationId = request.Id.HasValue ? request.Id.Value : Guid.Empty,
                LocationName = request.Name
            };
        }
    }
}