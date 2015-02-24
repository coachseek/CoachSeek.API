using AutoMapper;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api.Conversion
{
    public static class CoachAddCommandConverter
    {
        public static CoachAddCommand Convert(ApiCoachSaveCommand apiCommand)
        {
            return Mapper.Map<ApiCoachSaveCommand, CoachAddCommand>(apiCommand);
        }
    }
}