using AutoMapper;
using CoachSeek.Api.Models.Api.Out;
using CoachSeek.Data.Model;

namespace CoachSeek.Api.Conversion.Out
{
    public static class ApiOutSearchResponseConverter
    {
        public static ApiOutSessionSearchResult Convert(SessionSearchData sessionSearchData)
        {
            return Mapper.Map<SessionSearchData, ApiOutSessionSearchResult>(sessionSearchData);
        }
    }
}