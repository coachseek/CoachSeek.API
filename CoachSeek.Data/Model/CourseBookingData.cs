using System.Collections.Generic;

namespace CoachSeek.Data.Model
{
    public class CourseBookingData : BookingData
    {
        public SessionKeyData Course { get; set; }
        public IList<SingleSessionBookingData> SessionBookings { get; set; }
        // Possible future extension.
        //public IList<BookingSessionData> BookedSessions { get; set; }

        public CourseBookingData()
        {
            SessionBookings = new List<SingleSessionBookingData>();
            //BookedSessions = new List<BookingSessionData>();
        }
    }
}