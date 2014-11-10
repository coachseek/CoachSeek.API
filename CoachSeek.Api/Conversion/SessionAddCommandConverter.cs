using AutoMapper;
using CoachSeek.Api.Models.Api.Scheduling;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api.Conversion
{
    public class SessionAddCommandConverter
    {
        public static SessionAddCommand Convert(ApiSessionSaveCommand apiCommand)
        {
            return Mapper.Map<ApiSessionSaveCommand, SessionAddCommand>(apiCommand);
        }
    }
}