﻿using AutoMapper;
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
            Mapper.CreateMap<ApiBusinessAdminCommand, BusinessRegistrantCommand>();
            Mapper.CreateMap<ApiBusinessAdminCommand, UserAddCommand>();
            Mapper.CreateMap<ApiBusinessCommand, BusinessAddCommand>();
            Mapper.CreateMap<ApiBusinessRegistrationCommand, BusinessRegistrationCommand>();
            Mapper.CreateMap<ApiCoachKey, CoachKeyCommand>();
            Mapper.CreateMap<ApiCoachSaveCommand, CoachAddCommand>();
            Mapper.CreateMap<ApiCoachSaveCommand, CoachUpdateCommand>();
            Mapper.CreateMap<ApiCustomerSaveCommand, CustomerAddCommand>();
            Mapper.CreateMap<ApiCustomerSaveCommand, CustomerUpdateCommand>();
            Mapper.CreateMap<ApiDailyWorkingHours, DailyWorkingHoursCommand>();
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
            Mapper.CreateMap<ApiSessionSaveCommand, SessionAddCommand>();
            Mapper.CreateMap<ApiSessionSaveCommand, SessionUpdateCommand>();
            Mapper.CreateMap<ApiSessionTiming, SessionTimingCommand>();
            Mapper.CreateMap<ApiWeeklyWorkingHours, WeeklyWorkingHoursCommand>();
        }
    }
}