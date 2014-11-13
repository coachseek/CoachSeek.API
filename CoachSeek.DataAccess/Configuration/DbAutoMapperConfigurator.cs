﻿using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.DataAccess.Models;
using CoachSeek.Domain.Entities;

namespace CoachSeek.DataAccess.Configuration
{
    public static class DbAutoMapperConfigurator
    {
        public static void Configure()
        {
            Mapper.CreateMap<BusinessAdminData, DbBusinessAdmin>();
            Mapper.CreateMap<BusinessData, DbBusiness>();
            Mapper.CreateMap<CoachData, DbCoach>();
            Mapper.CreateMap<CoachKeyData, DbCoachKey>();
            Mapper.CreateMap<DailyWorkingHoursData, DbDailyWorkingHours>();
            Mapper.CreateMap<LocationData, DbLocation>();
            Mapper.CreateMap<LocationKeyData, DbLocationKey>();
            Mapper.CreateMap<PresentationData, DbPresentation>();
            Mapper.CreateMap<PricingData, DbPricing>();
            Mapper.CreateMap<RepetitionData, DbRepetition>();
            Mapper.CreateMap<ServiceData, DbService>();
            Mapper.CreateMap<ServiceDefaultsData, DbServiceDefaults>();
            Mapper.CreateMap<ServiceKeyData, DbServiceKey>();
            Mapper.CreateMap<SessionData, DbSession>();
            Mapper.CreateMap<SessionTimingData, DbSessionTiming>();
            Mapper.CreateMap<WeeklyWorkingHoursData, DbWeeklyWorkingHours>();


            Mapper.CreateMap<DbBusiness, BusinessData>();
            Mapper.CreateMap<DbBusinessAdmin, BusinessAdminData>();
            Mapper.CreateMap<DbCoach, CoachData>();
            Mapper.CreateMap<DbCoachKey, CoachKeyData>();
            Mapper.CreateMap<DbDailyWorkingHours, DailyWorkingHoursData>();
            Mapper.CreateMap<DbLocation, LocationData>();
            Mapper.CreateMap<DbLocationKey, LocationKeyData>();
            Mapper.CreateMap<DbPresentation, PresentationData>();
            Mapper.CreateMap<DbPricing, PricingData>();
            Mapper.CreateMap<DbRepetition, RepetitionData>();
            Mapper.CreateMap<DbService, ServiceData>();
            Mapper.CreateMap<DbServiceDefaults, ServiceDefaultsData>();
            Mapper.CreateMap<DbServiceKey, ServiceKeyData>();
            Mapper.CreateMap<DbSession, SessionData>();
            Mapper.CreateMap<DbSessionTiming, SessionTimingData>();
            Mapper.CreateMap<DbWeeklyWorkingHours, WeeklyWorkingHoursData>();
        }
    }
}
