using System;
using System.Collections.Generic;

namespace CoachSeek.Data.Model
{
    public class CourseBookingData : BookingData
    {
        public SessionKeyData Course { get; set; }

        public IList<SingleSessionBookingData> SessionBookings { get; set; }
    }
}