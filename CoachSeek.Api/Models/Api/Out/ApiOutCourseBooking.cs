using System.Collections.Generic;

namespace CoachSeek.Api.Models.Api.Out
{
    public class ApiOutCourseBooking : ApiOutBooking
    {
        public List<ApiOutCourseCustomerBooking> Bookings { get; set; }
    }
}