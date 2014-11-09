using AutoMapper;
using CoachSeek.Domain.Commands;
using CoachSeek.WebUI.Models.Api.Scheduling;

namespace CoachSeek.WebUI.Conversion
{
    public class SessionAddCommandConverter
    {
        public static SessionAddCommand Convert(ApiSessionSaveCommand apiCommand)
        {
            return Mapper.Map<ApiSessionSaveCommand, SessionAddCommand>(apiCommand);
        }
    }
}