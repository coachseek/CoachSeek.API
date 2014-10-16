using AutoMapper;
using CoachSeek.Domain.Commands;
using CoachSeek.WebUI.Models.Api;

namespace CoachSeek.WebUI
{
    public static class WebApiAutoMapperConfigurator
    {
        public static void Configure()
        {
            Mapper.CreateMap<ApiBusinessRegistrant, BusinessRegistrant>();
            Mapper.CreateMap<ApiBusinessRegistrationCommand, BusinessRegistrationCommand>();

            Mapper.CreateMap<ApiDailyWorkingHours, DailyWorkingHours>();
            Mapper.CreateMap<ApiWeeklyWorkingHours, WeeklyWorkingHours>();
            Mapper.CreateMap<ApiCoachSaveCommand, CoachAddCommand>();
        }
    }
}