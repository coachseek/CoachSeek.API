using AutoMapper;
using CoachSeek.Domain.Commands;
using CoachSeek.WebUI.Models.Api;

namespace CoachSeek.WebUI.Conversion
{
    public static class LocationAddCommandConverter
    {
        public static LocationAddCommand Convert(ApiLocationSaveCommand apiCommand)
        {
             return Mapper.Map<ApiLocationSaveCommand, LocationAddCommand>(apiCommand);
        }
    }
}