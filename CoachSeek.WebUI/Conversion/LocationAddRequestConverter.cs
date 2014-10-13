using CoachSeek.Domain.Commands;
using CoachSeek.WebUI.Models.Api;

namespace CoachSeek.WebUI.Conversion
{
    public static class LocationAddRequestConverter
    {
        public static LocationAddRequest Convert(ApiLocationSaveRequest apiRequest)
        {
            return new LocationAddRequest
            {
                BusinessId = apiRequest.BusinessId,
                LocationName = apiRequest.Name
            };
        }
    }
}