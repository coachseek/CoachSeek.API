using System;
using AutoMapper;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api.Conversion
{
    public static class CustomerUpdateCommandConverter
    {
        public static CustomerUpdateCommand Convert(Guid businessId, ApiCustomerSaveCommand apiCommand)
        {
            var command = Mapper.Map<ApiCustomerSaveCommand, CustomerUpdateCommand>(apiCommand);
            command.BusinessId = businessId;

            return command;
        }
    }
}