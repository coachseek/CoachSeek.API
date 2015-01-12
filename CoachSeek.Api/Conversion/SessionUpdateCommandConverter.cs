using System;
using AutoMapper;
using CoachSeek.Api.Models.Api.Scheduling;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api.Conversion
{
    public class SessionUpdateCommandConverter
    {
        public static SessionUpdateCommand Convert(Guid businessId, ApiSessionSaveCommand apiCommand)
        {
            var command = Mapper.Map<ApiSessionSaveCommand, SessionUpdateCommand>(apiCommand);
            command.BusinessId = businessId;

            return command;
        }
    }
}