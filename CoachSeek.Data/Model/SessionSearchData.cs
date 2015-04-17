using System.Collections.Generic;

namespace CoachSeek.Data.Model
{
    public class SessionSearchData
    {
        public IList<SingleSessionData> Sessions { get; set; }
        public IList<RepeatedSessionData> Courses { get; set; }


        public SessionSearchData(IList<SingleSessionData> sessions, IList<RepeatedSessionData> courses)
        {
            Sessions = sessions ?? new List<SingleSessionData>();
            Courses = courses ?? new List<RepeatedSessionData>();
        }
    }
}
