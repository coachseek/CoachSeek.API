using AutoMapper;
using CoachSeek.Domain.Commands;
using CoachSeek.WebUI.Models.Api.Setup;

namespace CoachSeek.WebUI.Conversion
{
    public static class CoachAddCommandConverter
    {
        public static CoachAddCommand Convert(ApiCoachSaveCommand apiCommand)
        {
            return Mapper.Map<ApiCoachSaveCommand, CoachAddCommand>(apiCommand);
        }
    }
}