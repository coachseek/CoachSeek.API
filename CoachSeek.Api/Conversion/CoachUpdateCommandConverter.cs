using System;
using AutoMapper;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api.Conversion
{
    public static class CoachUpdateCommandConverter
    {
        public static CoachUpdateCommand Convert(Guid businessId, ApiCoachSaveCommand apiCommand)
        {
            var command = Mapper.Map<ApiCoachSaveCommand, CoachUpdateCommand>(apiCommand);
            command.BusinessId = businessId;

            return command;
        }
    }
}