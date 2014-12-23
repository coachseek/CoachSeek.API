using AutoMapper;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api.Conversion
{
    public static class BusinessAddCommandConverter
    {
        public static BusinessAddCommand Convert(ApiBusinessCommand apiBusinessCommand)
        {
            return Mapper.Map<ApiBusinessCommand, BusinessAddCommand>(apiBusinessCommand);
        }
    }
}