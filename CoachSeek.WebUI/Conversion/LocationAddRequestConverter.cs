using CoachSeek.WebUI.Models.Api;
using CoachSeek.WebUI.Models.Requests;
using CoachSeek.WebUI.Models.UseCases.Requests;

namespace CoachSeek.WebUI.Conversion
{
    public static class LocationAddRequestConverter
    {
        public static LocationAddRequest Convert(ApiLocationSaveRequest locationSaveRequest)
        {
            return new LocationAddRequest
            {
                BusinessId = locationSaveRequest.BusinessId,
                LocationName = locationSaveRequest.Name
            };
        }
    }
}