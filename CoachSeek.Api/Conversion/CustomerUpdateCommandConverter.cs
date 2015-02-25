using AutoMapper;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api.Conversion
{
    public static class CustomerUpdateCommandConverter
    {
        public static CustomerUpdateCommand Convert(ApiCustomerSaveCommand apiCommand)
        {
            return Mapper.Map<ApiCustomerSaveCommand, CustomerUpdateCommand>(apiCommand);
        }
    }
}