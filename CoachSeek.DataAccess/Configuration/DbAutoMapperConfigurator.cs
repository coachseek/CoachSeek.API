using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.DataAccess.Models;

namespace CoachSeek.DataAccess.Configuration
{
    public static class DbAutoMapperConfigurator
    {
        public static void Configure()
        {
            Mapper.CreateMap<BusinessData, DbBusiness>();
            Mapper.CreateMap<BusinessAdminData, DbBusinessAdmin>();
            Mapper.CreateMap<LocationData, DbLocation>();
            Mapper.CreateMap<CoachData, DbCoach>();
            Mapper.CreateMap<WeeklyWorkingHoursData, DbWeeklyWorkingHours>();
            Mapper.CreateMap<DailyWorkingHoursData, DbDailyWorkingHours>();
            Mapper.CreateMap<ServiceData, DbService>();

            Mapper.CreateMap<DbBusiness, BusinessData>();
            Mapper.CreateMap<DbBusinessAdmin, BusinessAdminData>();
            Mapper.CreateMap<DbLocation, LocationData>();
            Mapper.CreateMap<DbCoach, CoachData>();
            Mapper.CreateMap<DbWeeklyWorkingHours, WeeklyWorkingHoursData>();
            Mapper.CreateMap<DbDailyWorkingHours, DailyWorkingHoursData>();
            Mapper.CreateMap<DbService, ServiceData>();
        }
    }
}
