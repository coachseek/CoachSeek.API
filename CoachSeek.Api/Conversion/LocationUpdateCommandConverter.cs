using AutoMapper;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api.Conversion
{
    public static class LocationUpdateCommandConverter
    {
        public static LocationUpdateCommand Convert(ApiLocationSaveCommand apiCommand)
        {
            return Mapper.Map<ApiLocationSaveCommand, LocationUpdateCommand>(apiCommand);
        }
    }
}