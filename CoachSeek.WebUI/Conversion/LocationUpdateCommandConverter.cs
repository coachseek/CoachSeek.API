using AutoMapper;
using CoachSeek.Domain.Commands;
using CoachSeek.WebUI.Models.Api;
using CoachSeek.WebUI.Models.Api.Setup;

namespace CoachSeek.WebUI.Conversion
{
    public static class LocationUpdateCommandConverter
    {
        public static LocationUpdateCommand Convert(ApiLocationSaveCommand apiCommand)
        {
            return Mapper.Map<ApiLocationSaveCommand, LocationUpdateCommand>(apiCommand);
        }
    }
}