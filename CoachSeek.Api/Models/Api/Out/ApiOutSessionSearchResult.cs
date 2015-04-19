using System.Collections.Generic;

namespace CoachSeek.Api.Models.Api.Out
{
    public class ApiOutSessionSearchResult
    {
        public IList<ApiOutSingleSession> Sessions { get; set; }
        public IList<ApiOutCourse> Courses { get; set; }
    }
}