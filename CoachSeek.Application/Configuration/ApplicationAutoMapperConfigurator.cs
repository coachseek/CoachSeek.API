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
            Mapper.CreateMap<DailyWorkingHours, DailyWorkingHoursData>();
            Mapper.CreateMap<Error, ErrorData>();
            Mapper.CreateMap<Location, LocationData>();
            Mapper.CreateMap<Location, LocationKeyData>();
            Mapper.CreateMap<ServicePresentation, PresentationData>();
            Mapper.CreateMap<Service, ServiceData>();
            Mapper.CreateMap<Service, ServiceKeyData>();
            Mapper.CreateMap<ServiceBooking, ServiceBookingData>();
            Mapper.CreateMap<ServicePricing, PricingData>();
            Mapper.CreateMap<ServiceRepetition, RepetitionData>();
            Mapper.CreateMap<ServiceTiming, ServiceTimingData>(); 
            Mapper.CreateMap<Session, SessionData>();
            Mapper.CreateMap<SessionBooking, SessionBookingData>();
            Mapper.CreateMap<SessionPresentation, PresentationData>();
            Mapper.CreateMap<SessionPricing, PricingData>();
            Mapper.CreateMap<SessionRepetition, RepetitionData>();
            Mapper.CreateMap<SessionTiming, SessionTimingData>();
            Mapper.CreateMap<WeeklyWorkingHours, WeeklyWorkingHoursData>();


            Mapper.CreateMap<CoachAddCommand, NewCoachData>();
            Mapper.CreateMap<CoachKeyCommand, CoachKeyData>();
            Mapper.CreateMap<CoachUpdateCommand, CoachData>();
            Mapper.CreateMap<DailyWorkingHoursCommand, DailyWorkingHoursData>();
            Mapper.CreateMap<LocationAddCommand, NewLocationData>();
            Mapper.CreateMap<LocationKeyCommand, LocationKeyData>();
            Mapper.CreateMap<LocationUpdateCommand, LocationData>();
            Mapper.CreateMap<PresentationCommand, PresentationData>();
            Mapper.CreateMap<PricingCommand, PricingData>();
            Mapper.CreateMap<RepetitionCommand, RepetitionData>();
            Mapper.CreateMap<ServiceAddCommand, NewServiceData>();
            Mapper.CreateMap<ServiceBookingCommand, ServiceBookingData>();
            Mapper.CreateMap<ServiceKeyCommand, ServiceKeyData>();
            Mapper.CreateMap<ServiceTimingCommand, ServiceTimingData>();
            Mapper.CreateMap<ServiceUpdateCommand, ServiceData>();
            Mapper.CreateMap<SessionAddCommand, NewSessionData>();
            Mapper.CreateMap<SessionBookingCommand, SessionBookingData>();
            Mapper.CreateMap<SessionTimingCommand, SessionTimingData>();
            Mapper.CreateMap<WeeklyWorkingHoursCommand, WeeklyWorkingHoursData>();
        }
    }
}
