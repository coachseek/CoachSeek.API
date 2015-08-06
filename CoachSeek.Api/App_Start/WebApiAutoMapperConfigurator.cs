using AutoMapper;
using CoachSeek.Api.Models.Api;
using CoachSeek.Api.Models.Api.Booking;
using CoachSeek.Api.Models.Api.Out;
using CoachSeek.Api.Models.Api.Scheduling;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api
{
    public static class WebApiAutoMapperConfigurator
    {
        public static void Configure()
        {
            // In
            Mapper.CreateMap<ApiBookingSaveCommand, BookingAddCommand>();
            Mapper.CreateMap<ApiBusinessAdminCommand, BusinessRegistrantCommand>();
            Mapper.CreateMap<ApiBusinessAdminCommand, UserAddCommand>();
            Mapper.CreateMap<ApiBusinessCommand, BusinessAddCommand>();
            Mapper.CreateMap<ApiBusinessPaymentOptions, BusinessPaymentCommand>();
            Mapper.CreateMap<ApiBusinessRegistrationCommand, BusinessRegistrationCommand>();
            Mapper.CreateMap<ApiBusinessSaveCommand, BusinessUpdateCommand>();
            Mapper.CreateMap<ApiCoachKey, CoachKeyCommand>();
            Mapper.CreateMap<ApiCoachSaveCommand, CoachAddCommand>();
            Mapper.CreateMap<ApiCoachSaveCommand, CoachUpdateCommand>();
            Mapper.CreateMap<ApiCustomerKey, CustomerKeyCommand>();
            Mapper.CreateMap<ApiCustomerSaveCommand, CustomerAddCommand>();
            Mapper.CreateMap<ApiCustomerSaveCommand, CustomerUpdateCommand>();
            Mapper.CreateMap<ApiDailyWorkingHours, DailyWorkingHoursCommand>();
            Mapper.CreateMap<ApiEmailTemplateSaveCommand, EmailTemplateUpdateCommand>();
            Mapper.CreateMap<ApiLocationKey, LocationKeyCommand>();
            Mapper.CreateMap<ApiLocationSaveCommand, LocationAddCommand>();
            Mapper.CreateMap<ApiLocationSaveCommand, LocationUpdateCommand>();
            Mapper.CreateMap<ApiPresentation, PresentationCommand>();
            Mapper.CreateMap<ApiPricing, PricingCommand>();
            Mapper.CreateMap<ApiRepetition, RepetitionCommand>();
            Mapper.CreateMap<ApiServiceBooking, ServiceBookingCommand>();
            Mapper.CreateMap<ApiServiceKey, ServiceKeyCommand>();
            Mapper.CreateMap<ApiServiceSaveCommand, ServiceAddCommand>();
            Mapper.CreateMap<ApiServiceSaveCommand, ServiceUpdateCommand>();
            Mapper.CreateMap<ApiServiceTiming, ServiceTimingCommand>();
            Mapper.CreateMap<ApiSessionBooking, SessionBookingCommand>();
            Mapper.CreateMap<ApiSessionKey, SessionKeyCommand>();
            Mapper.CreateMap<ApiSessionSaveCommand, SessionAddCommand>();
            Mapper.CreateMap<ApiSessionSaveCommand, SessionUpdateCommand>();
            Mapper.CreateMap<ApiSessionTiming, SessionTimingCommand>();
            Mapper.CreateMap<ApiWeeklyWorkingHours, WeeklyWorkingHoursCommand>();


            // Out
            Mapper.CreateMap<SessionSearchData, ApiOutSessionSearchResult>();
            Mapper.CreateMap<SingleSessionData, ApiOutSingleSession>();
            Mapper.CreateMap<SessionBookingData, ApiOutSessionBooking>();
            Mapper.CreateMap<CustomerBookingData, ApiOutSessionCustomerBooking>();
            Mapper.CreateMap<RepeatedSessionData, ApiOutCourse>();
            Mapper.CreateMap<SessionBookingData, ApiOutCourseBooking>();
            Mapper.CreateMap<CustomerBookingData, ApiOutCourseCustomerBooking>();

            Mapper.CreateMap<SessionSearchData, ApiOutOnlineBookingSessionSearchResult>();
            Mapper.CreateMap<SingleSessionData, ApiOutOnlineBookingSingleSession>();
            Mapper.CreateMap<SessionBookingData, ApiOutOnlineBookingSessionBooking>();
            Mapper.CreateMap<RepeatedSessionData, ApiOutOnlineBookingCourse>();
            Mapper.CreateMap<SessionBookingData, ApiOutOnlineBookingCourseBooking>();
        }
    }
}