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
            Mapper.CreateMap<Location, LocationData>();
            Mapper.CreateMap<WeeklyWorkingHours, WeeklyWorkingHoursData>();
            Mapper.CreateMap<DailyWorkingHours, DailyWorkingHoursData>();

            Mapper.CreateMap<CoachAddCommand, NewCoachData>();
            Mapper.CreateMap<CoachUpdateCommand, CoachData>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CoachId));
            Mapper.CreateMap<WeeklyWorkingHoursCommand, WeeklyWorkingHoursData>();
            Mapper.CreateMap<DailyWorkingHoursCommand, DailyWorkingHoursData>();

            Mapper.CreateMap<LocationAddCommand, NewLocationData>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.LocationName));
            Mapper.CreateMap<LocationUpdateCommand, LocationData>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.LocationId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.LocationName));

            Mapper.CreateMap<Error, ErrorData>();
        }
    }
}
