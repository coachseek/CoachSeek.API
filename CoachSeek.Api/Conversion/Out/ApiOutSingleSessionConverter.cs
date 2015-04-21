using AutoMapper;
using CoachSeek.Api.Models.Api.Out;
using CoachSeek.Data.Model;

namespace CoachSeek.Api.Conversion.Out
{
    public static class ApiOutSingleSessionConverter
    {
        public static ApiOutSingleSession Convert(SingleSessionData singleSessionData)
        {
            return Mapper.Map<SingleSessionData, ApiOutSingleSession>(singleSessionData);
        }
    }
}