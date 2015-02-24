using AutoMapper;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api.Conversion
{
    public static class CoachUpdateCommandConverter
    {
        public static CoachUpdateCommand Convert(ApiCoachSaveCommand apiCommand)
        {
            return Mapper.Map<ApiCoachSaveCommand, CoachUpdateCommand>(apiCommand);
        }
    }
}