using AutoMapper;
using CoachSeek.Api.Models.Api.Scheduling;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api.Conversion
{
    public class SessionUpdateCommandConverter
    {
        public static SessionUpdateCommand Convert(ApiSessionSaveCommand apiCommand)
        {
            return Mapper.Map<ApiSessionSaveCommand, SessionUpdateCommand>(apiCommand);
        }
    }
}