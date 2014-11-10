using AutoMapper;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api.Conversion
{
    public static class BusinessAddCommandConverter
    {
        public static BusinessRegistrationCommand Convert(ApiBusinessRegistrationCommand apiBusinessRegistrationCommand)
        {
            return Mapper.Map<ApiBusinessRegistrationCommand, BusinessRegistrationCommand>(apiBusinessRegistrationCommand);
        }
    }
}