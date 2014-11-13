using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Application.Configuration
{
    public static class ApplicationAutoMapperConfigurator
    {
        public static void Configure()
        {
            Mapper.CreateMap<Business, BusinessData>();
            Mapper.CreateMap<BusinessAdmin, BusinessAdminData>();
            Mapper.CreateMap<Coach, CoachData>();
            Mapper.CreateMap<Coach, CoachKeyData>();
            Mapper.CreateMap<Location, LocationData>();
            Mapper.CreateMap<Location, LocationKeyData>();
            Mapper.CreateMap<WeeklyWorkingHours, WeeklyWorkingHoursData>();
            Mapper.CreateMap<DailyWorkingHours, DailyWorkingHoursData>();
            Mapper.CreateMap<Service, ServiceData>();
            Mapper.CreateMap<Service, ServiceKeyData>();
            Mapper.CreateMap<ServiceDefaults, ServiceDefaultsData>();
            Mapper.CreateMap<ServicePricing, PricingData>();
            Mapper.CreateMap<ServiceRepetition, RepetitionData>();
            Mapper.CreateMap<Session, SessionData>();
            Mapper.CreateMap<SessionTiming, SessionTimingData>();
            Mapper.CreateMap<Presentation, PresentationData>();

            Mapper.CreateMap<CoachAddCommand, NewCoachData>();
            Mapper.CreateMap<CoachUpdateCommand, CoachData>();
            Mapper.CreateMap<WeeklyWorkingHoursCommand, WeeklyWorkingHoursData>();
            Mapper.CreateMap<DailyWorkingHoursCommand, DailyWorkingHoursData>();

            Mapper.CreateMap<LocationAddCommand, NewLocationData>();
            Mapper.CreateMap<LocationUpdateCommand, LocationData>();

            Mapper.CreateMap<ServiceAddCommand, NewServiceData>();
            Mapper.CreateMap<ServiceUpdateCommand, ServiceData>();
            Mapper.CreateMap<ServiceDefaultsCommand, ServiceDefaultsData>();
            Mapper.CreateMap<PricingCommand, PricingData>();
            Mapper.CreateMap<RepetitionCommand, RepetitionData>();

            Mapper.CreateMap<SessionAddCommand, NewSessionData>();
            Mapper.CreateMap<ServiceKeyCommand, ServiceKeyData>();
            Mapper.CreateMap<LocationKeyCommand, LocationKeyData>();
            Mapper.CreateMap<CoachKeyCommand, CoachKeyData>();
            Mapper.CreateMap<SessionTimingCommand, SessionTimingData>();
            Mapper.CreateMap<SessionBookingCommand, SessionBookingData>();
            Mapper.CreateMap<PresentationCommand, PresentationData>();

            Mapper.CreateMap<Error, ErrorData>();
        }
    }
}
