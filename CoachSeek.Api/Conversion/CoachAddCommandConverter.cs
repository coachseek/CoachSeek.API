using System;
using AutoMapper;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api.Conversion
{
    public static class CoachAddCommandConverter
    {
        public static CoachAddCommand Convert(Guid businessId, ApiCoachSaveCommand apiCommand)
        {
            var command = Mapper.Map<ApiCoachSaveCommand, CoachAddCommand>(apiCommand);
            command.BusinessId = businessId;

            return command;
        }
    }
}