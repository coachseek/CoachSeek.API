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
            Mapper.CreateMap<BusinessAdmin, BusinessAdminData>();
            Mapper.CreateMap<Coach, CoachData>();
            Mapper.CreateMap<Coach, CoachKeyData>();
            Mapper.CreateMap<Customer, CustomerData>();
            Mapper.CreateMap<DailyWorkingHours, DailyWorkingHoursData>();
            Mapper.CreateMap<Error, ErrorData>();
            Mapper.CreateMap<Location, LocationData>();
            Mapper.CreateMap<Location, LocationKeyData>();
            Mapper.CreateMap<RepeatedSessionPricing, RepeatedSessionPricingData>();
            Mapper.CreateMap<ServicePresentation, PresentationData>();
            Mapper.CreateMap<Service, ServiceData>();
            Mapper.CreateMap<Service, ServiceKeyData>();
            Mapper.CreateMap<ServiceBooking, ServiceBookingData>();
            Mapper.CreateMap<ServicePricing, RepeatedSessionPricingData>();
            Mapper.CreateMap<ServiceRepetition, RepetitionData>();
            Mapper.CreateMap<ServiceTiming, ServiceTimingData>(); 
            Mapper.CreateMap<Session, SessionData>();
            Mapper.CreateMap<SessionBooking, SessionBookingData>();
            Mapper.CreateMap<SessionPresentation, PresentationData>();
            Mapper.CreateMap<SessionRepetition, RepetitionData>();
            Mapper.CreateMap<SessionTiming, SessionTimingData>();
            Mapper.CreateMap<SingleSessionPricing, SingleSessionPricingData>();
            Mapper.CreateMap<WeeklyWorkingHours, WeeklyWorkingHoursData>();


            Mapper.CreateMap<User, UserData>();

            Mapper.CreateMap<SingleSession, SingleSessionData>();
            Mapper.CreateMap<StandaloneSession, SingleSessionData>();
            Mapper.CreateMap<RepeatedSession, RepeatedSessionData>();
            Mapper.CreateMap<RepeatedSession, SingleSessionData>();
            Mapper.CreateMap<RepetitionData, SingleRepetitionData>();


            Mapper.CreateMap<CoachKeyCommand, CoachKeyData>();
            Mapper.CreateMap<CoachUpdateCommand, CoachData>();
            Mapper.CreateMap<CustomerUpdateCommand, CustomerData>();
            Mapper.CreateMap<DailyWorkingHoursCommand, DailyWorkingHoursData>();
            Mapper.CreateMap<LocationKeyCommand, LocationKeyData>();
            Mapper.CreateMap<LocationUpdateCommand, LocationData>();
            Mapper.CreateMap<PresentationCommand, PresentationData>();
            Mapper.CreateMap<PricingCommand, RepeatedSessionPricingData>();
            Mapper.CreateMap<RepetitionCommand, RepetitionData>();
            Mapper.CreateMap<ServiceBookingCommand, ServiceBookingData>();
            Mapper.CreateMap<ServiceKeyCommand, ServiceKeyData>();
            Mapper.CreateMap<ServiceTimingCommand, ServiceTimingData>();
            Mapper.CreateMap<ServiceUpdateCommand, ServiceData>();
            Mapper.CreateMap<SessionBookingCommand, SessionBookingData>();
            Mapper.CreateMap<SessionTimingCommand, SessionTimingData>();
            Mapper.CreateMap<SessionUpdateCommand, SessionData>();
            Mapper.CreateMap<WeeklyWorkingHoursCommand, WeeklyWorkingHoursData>();
        }
    }
}
