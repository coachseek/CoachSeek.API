using System;
using AutoMapper;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api.Conversion
{
    public class ServiceUpdateCommandConverter
    {
        public static ServiceUpdateCommand Convert(Guid businessId, ApiServiceSaveCommand apiCommand)
        {
            var command = Mapper.Map<ApiServiceSaveCommand, ServiceUpdateCommand>(apiCommand);
            command.BusinessId = businessId;

            return command;
        }
    }
}