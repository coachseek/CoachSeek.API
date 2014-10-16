using AutoMapper;
using CoachSeek.Domain.Commands;
using CoachSeek.WebUI.Models.Api;

namespace CoachSeek.WebUI.Conversion
{
    public static class CoachAddCommandConverter
    {
        //static CoachAddCommandConverter()
        //{
        //    Mapper.CreateMap<ApiDailyWorkingHours, DailyWorkingHours>();
        //    Mapper.CreateMap<ApiWeeklyWorkingHours, WeeklyWorkingHours>();
        //    Mapper.CreateMap<ApiCoachSaveCommand, CoachAddCommand>();
        //}

        public static CoachAddCommand Convert(ApiCoachSaveCommand apiCommand)
        {
            return Mapper.Map<ApiCoachSaveCommand, CoachAddCommand>(apiCommand);
        }
    }
}