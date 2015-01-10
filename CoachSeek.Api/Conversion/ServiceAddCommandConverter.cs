using System;
using AutoMapper;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api.Conversion
{
    public class ServiceAddCommandConverter
    {
        public static ServiceAddCommand Convert(Guid businessId, ApiServiceSaveCommand apiCommand)
        {
            var command = Mapper.Map<ApiServiceSaveCommand, ServiceAddCommand>(apiCommand);
            command.BusinessId = businessId;

            return command;
        }
    }
}