using System;
using AutoMapper;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api.Conversion
{
    public static class LocationAddCommandConverter
    {
        public static LocationAddCommand Convert(Guid businessId, ApiLocationSaveCommand apiCommand)
        {
             var command = Mapper.Map<ApiLocationSaveCommand, LocationAddCommand>(apiCommand);
            command.BusinessId = businessId;

            return command;
        }
    }
}