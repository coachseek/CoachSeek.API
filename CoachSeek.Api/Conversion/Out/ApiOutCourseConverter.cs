using AutoMapper;
using CoachSeek.Api.Models.Api.Out;
using CoachSeek.Data.Model;

namespace CoachSeek.Api.Conversion.Out
{
    public static class ApiOutCourseConverter
    {
        public static ApiOutCourse Convert(RepeatedSessionData courseData)
        {
            return Mapper.Map<RepeatedSessionData, ApiOutCourse>(courseData);
        }
    }
}