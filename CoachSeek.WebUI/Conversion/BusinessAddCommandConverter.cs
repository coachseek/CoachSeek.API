using AutoMapper;
using CoachSeek.Domain.Commands;
using CoachSeek.WebUI.Models.Api;

namespace CoachSeek.WebUI.Conversion
{
    public static class BusinessAddCommandConverter
    {
        //static BusinessAddCommandConverter()
        //{
        //    AutoMapper.Mapper.CreateMap<ApiBusinessRegistrant, BusinessRegistrant>();
        //    AutoMapper.Mapper.CreateMap<ApiBusinessRegistrationCommand, BusinessRegistrationCommand>();
        //}

        public static BusinessRegistrationCommand Convert(ApiBusinessRegistrationCommand apiBusinessRegistrationCommand)
        {
            return Mapper.Map<ApiBusinessRegistrationCommand, BusinessRegistrationCommand>(apiBusinessRegistrationCommand);
        }
    }
}