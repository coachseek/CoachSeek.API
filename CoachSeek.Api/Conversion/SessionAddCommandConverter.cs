using System;
using AutoMapper;
using CoachSeek.Api.Models.Api.Scheduling;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api.Conversion
{
    public class SessionAddCommandConverter
    {
        public static SessionAddCommand Convert(Guid businessId, ApiSessionSaveCommand apiCommand)
        {
            var command = Mapper.Map<ApiSessionSaveCommand, SessionAddCommand>(apiCommand);
            command.BusinessId = businessId;

            return command;
        }
    }
}