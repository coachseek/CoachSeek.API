using CoachSeek.WebUI.Models.Api;
using CoachSeek.WebUI.Models.UseCases.Requests;

namespace CoachSeek.WebUI.Conversion
{
    public class LocationUpdateRequestConverter
    {
        public static LocationUpdateRequest Convert(ApiLocationSaveRequest locationSaveRequest)
        {
            return new LocationUpdateRequest
            {
                BusinessId = locationSaveRequest.BusinessId,
                LocationId = locationSaveRequest.Id,
                LocationName = locationSaveRequest.Name
            };
        }
    }
}