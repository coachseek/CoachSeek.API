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

            Mapper.CreateMap<ApiLocationSaveCommand, LocationAddCommand>();
            Mapper.CreateMap<ApiLocationSaveCommand, LocationUpdateCommand>();

            Mapper.CreateMap<ApiDailyWorkingHours, DailyWorkingHoursCommand>();
            Mapper.CreateMap<ApiWeeklyWorkingHours, WeeklyWorkingHoursCommand>();
            Mapper.CreateMap<ApiCoachSaveCommand, CoachAddCommand>();
            Mapper.CreateMap<ApiCoachSaveCommand, CoachUpdateCommand>();

            Mapper.CreateMap<ApiServiceDefaults, ServiceDefaultsCommand>();
            Mapper.CreateMap<ApiServicePricing, ServicePricingCommand>();
            Mapper.CreateMap<ApiServiceRepetition, ServiceRepetitionCommand>();
            Mapper.CreateMap<ApiServiceSaveCommand, ServiceAddCommand>();
            Mapper.CreateMap<ApiServiceSaveCommand, ServiceUpdateCommand>();
        }
    }
}