using CoachSeek.Domain.Commands;
using CoachSeek.WebUI.Models.Api;

namespace CoachSeek.WebUI.Conversion
{
    public static class LocationAddCommandConverter
    {
        public static LocationAddCommand Convert(ApiLocationSaveCommand apiCommand)
        {
            return new LocationAddCommand
            {
                BusinessId = apiCommand.BusinessId.Value,
                LocationName = apiCommand.Name
            };
        }
    }
}