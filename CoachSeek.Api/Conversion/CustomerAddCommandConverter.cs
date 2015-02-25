using AutoMapper;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api.Conversion
{
    public static class CustomerAddCommandConverter
    {
        public static CustomerAddCommand Convert(ApiCustomerSaveCommand apiCommand)
        {
            return Mapper.Map<ApiCustomerSaveCommand, CustomerAddCommand>(apiCommand);
        }
    }
}