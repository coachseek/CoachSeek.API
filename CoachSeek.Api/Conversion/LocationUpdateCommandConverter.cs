using System;
using AutoMapper;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api.Conversion
{
    public static class LocationUpdateCommandConverter
    {
        public static LocationUpdateCommand Convert(Guid businessId, ApiLocationSaveCommand apiCommand)
        {
            var command = Mapper.Map<ApiLocationSaveCommand, LocationUpdateCommand>(apiCommand);
            command.BusinessId = businessId;

            return command;
        }
    }
}