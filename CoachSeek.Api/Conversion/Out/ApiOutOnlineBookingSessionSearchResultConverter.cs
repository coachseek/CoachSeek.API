using AutoMapper;
using CoachSeek.Api.Models.Api.Out;
using CoachSeek.Data.Model;

namespace CoachSeek.Api.Conversion.Out
{
    public static class ApiOutOnlineBookingSessionSearchResultConverter
    {
        public static ApiOutOnlineBookingSessionSearchResult Convert(SessionSearchData sessionSearchData)
        {
            return Mapper.Map<SessionSearchData, ApiOutOnlineBookingSessionSearchResult>(sessionSearchData);
        }
    }
}