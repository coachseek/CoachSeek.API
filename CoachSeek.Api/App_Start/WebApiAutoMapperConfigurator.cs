using AutoMapper;
using CoachSeek.Api.Models.Api;
using CoachSeek.Api.Models.Api.Scheduling;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api
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
            Mapper.CreateMap<ApiPricing, PricingCommand>();
            Mapper.CreateMap<ApiRepetition, RepetitionCommand>();
            Mapper.CreateMap<ApiServiceSaveCommand, ServiceAddCommand>();
            Mapper.CreateMap<ApiServiceSaveCommand, ServiceUpdateCommand>();

            Mapper.CreateMap<ApiSessionSaveCommand, SessionAddCommand>();
            Mapper.CreateMap<ApiServiceKey, ServiceKeyCommand>();
            Mapper.CreateMap<ApiLocationKey, LocationKeyCommand>();
            Mapper.CreateMap<ApiCoachKey, CoachKeyCommand>();

            Mapper.CreateMap<ApiSessionTiming, SessionTimingCommand>();
            Mapper.CreateMap<ApiSessionBooking, SessionBookingCommand>();
            Mapper.CreateMap<ApiPricing, PricingCommand>();
            Mapper.CreateMap<ApiRepetition, RepetitionCommand>();
            Mapper.CreateMap<ApiPresentation, PresentationCommand>();
        }
    }
}