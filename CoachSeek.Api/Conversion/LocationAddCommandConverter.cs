using AutoMapper;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api.Conversion
{
    public static class LocationAddCommandConverter
    {
        public static LocationAddCommand Convert(ApiLocationSaveCommand apiCommand)
        {
            return Mapper.Map<ApiLocationSaveCommand, LocationAddCommand>(apiCommand);
        }
    }
}