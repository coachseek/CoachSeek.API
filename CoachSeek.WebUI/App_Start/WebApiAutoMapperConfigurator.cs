using AutoMapper;
using CoachSeek.Domain.Commands;
using CoachSeek.WebUI.Models.Api;

namespace CoachSeek.WebUI
{
    public static class WebApiAutoMapperConfigurator
    {
        public static void Configure()
        {
            Mapper.CreateMap<ApiBusinessRegistrant, BusinessRegistrantCommand>();
            Mapper.CreateMap<ApiBusinessRegistrationCommand, BusinessRegistrationCommand>();

            Mapper.CreateMap<ApiDailyWorkingHours, DailyWorkingHoursCommand>();
            Mapper.CreateMap<ApiWeeklyWorkingHours, WeeklyWorkingHoursCommand>();
            Mapper.CreateMap<ApiCoachSaveCommand, CoachAddCommand>();
        }
    }
}