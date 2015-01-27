using System;
using AutoMapper;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api.Conversion
{
    public static class CustomerAddCommandConverter
    {
        public static CustomerAddCommand Convert(Guid businessId, ApiCustomerSaveCommand apiCommand)
        {
            var command = Mapper.Map<ApiCustomerSaveCommand, CustomerAddCommand>(apiCommand);
            command.BusinessId = businessId;

            return command;
        }
    }
}