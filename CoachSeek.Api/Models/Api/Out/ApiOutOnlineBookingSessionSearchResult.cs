using System.Collections.Generic;

namespace CoachSeek.Api.Models.Api.Out
{
    public class ApiOutOnlineBookingSessionSearchResult
    {
        public IList<ApiOutOnlineBookingSingleSession> Sessions { get; set; }
        public IList<ApiOutOnlineBookingCourse> Courses { get; set; }
    }
}