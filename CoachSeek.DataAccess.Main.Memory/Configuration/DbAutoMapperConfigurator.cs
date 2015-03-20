using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.DataAccess.Authentication.Models;
using CoachSeek.DataAccess.Main.Memory.Models;
using CoachSeek.DataAccess.Models;
using CoachSeek.Domain.Entities;

namespace CoachSeek.DataAccess.Main.Memory.Configuration
{
    public static class DbAutoMapperConfigurator
    {
        public static void Configure()
        {
            Mapper.CreateMap<BusinessAdminData, DbBusinessAdmin>();
            Mapper.CreateMap<Booking, DbBooking>();
            Mapper.CreateMap<Business, DbBusiness>();
            Mapper.CreateMap<Coach, DbCoach>();
            Mapper.CreateMap<CoachData, DbCoach>();
            Mapper.CreateMap<CoachKeyData, DbCoachKey>();
            Mapper.CreateMap<CustomerData, DbCustomer>();
            Mapper.CreateMap<Customer, DbCustomer>();
            Mapper.CreateMap<CustomerKeyData, DbCustomerKey>();
            Mapper.CreateMap<DailyWorkingHoursData, DbDailyWorkingHours>();
            Mapper.CreateMap<Location, DbLocation>();
            Mapper.CreateMap<LocationData, DbLocation>();
            Mapper.CreateMap<LocationKeyData, DbLocationKey>();
            Mapper.CreateMap<PresentationData, DbPresentation>();
            Mapper.CreateMap<RepetitionData, DbRepetition>();
            Mapper.CreateMap<ServiceBookingData, DbServiceBooking>();
            Mapper.CreateMap<Service, DbService>();
            Mapper.CreateMap<ServiceData, DbService>();
            Mapper.CreateMap<ServiceKeyData, DbServiceKey>();
            Mapper.CreateMap<ServiceTimingData, DbServiceTiming>();
            Mapper.CreateMap<SessionBookingData, DbSessionBooking>();
            Mapper.CreateMap<SessionKeyData, DbSessionKey>();

            Mapper.CreateMap<StandaloneSession, DbSingleSession>();
            Mapper.CreateMap<SingleSession, DbSingleSession>();
            Mapper.CreateMap<RepeatedSession, DbRepeatedSession>();

            Mapper.CreateMap<SingleSessionData, DbSingleSession>();
            Mapper.CreateMap<RepeatedSessionData, DbRepeatedSession>();

            Mapper.CreateMap<SingleSessionPricingData, DbSingleSessionPricing>();
            Mapper.CreateMap<RepeatedSessionPricingData, DbRepeatedSessionPricing>();

            Mapper.CreateMap<SessionTimingData, DbSessionTiming>();
            Mapper.CreateMap<SingleSessionPricingData, DbSingleSessionPricing>();
            Mapper.CreateMap<UserData, DbUser>();
            Mapper.CreateMap<WeeklyWorkingHoursData, DbWeeklyWorkingHours>();

            Mapper.CreateMap<SingleRepetitionData, DbRepetition>();

            Mapper.CreateMap<DbRepetition, SingleRepetitionData>();


            Mapper.CreateMap<DbSingleSession, SingleSessionData>();
            Mapper.CreateMap<DbRepeatedSession, RepeatedSessionData>();

            Mapper.CreateMap<DbSingleSessionPricing, SingleSessionPricingData>();
            Mapper.CreateMap<DbRepeatedSessionPricing, RepeatedSessionPricingData>();



            Mapper.CreateMap<DbBooking, BookingData>();
            Mapper.CreateMap<DbBusiness, BusinessData>();
            Mapper.CreateMap<DbBusinessAdmin, BusinessAdminData>();
            Mapper.CreateMap<DbCoach, CoachData>();
            Mapper.CreateMap<DbCoachKey, CoachKeyData>();
            Mapper.CreateMap<DbCustomer, CustomerData>();
            Mapper.CreateMap<DbCustomerKey, CustomerKeyData>();
            Mapper.CreateMap<DbDailyWorkingHours, DailyWorkingHoursData>();
            Mapper.CreateMap<DbLocation, LocationData>();
            Mapper.CreateMap<DbLocationKey, LocationKeyData>();
            Mapper.CreateMap<DbPresentation, PresentationData>();


            Mapper.CreateMap<DbRepetition, RepetitionData>();
            Mapper.CreateMap<DbService, ServiceData>();
            Mapper.CreateMap<DbServiceBooking, ServiceBookingData>();
            Mapper.CreateMap<DbServiceKey, ServiceKeyData>();
            Mapper.CreateMap<DbServiceTiming, ServiceTimingData>(); 
            Mapper.CreateMap<DbSingleSession, SingleSessionData>();
            Mapper.CreateMap<DbSessionBooking, SessionBookingData>();
            Mapper.CreateMap<DbSessionKey, SessionKeyData>();
            Mapper.CreateMap<DbSessionTiming, SessionTimingData>();
            Mapper.CreateMap<DbUser, UserData>();
            Mapper.CreateMap<DbWeeklyWorkingHours, WeeklyWorkingHoursData>();
        }
    }
}
