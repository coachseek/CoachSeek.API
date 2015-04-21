using System.Collections.Generic;
using CoachSeek.Data.Model;

namespace CoachSeek.Api.Models.Api.Out
{
    public class ApiOutOnlineBookingCourse : ApiOutSession
    {
        public ApiOutOnlineBookingCourseBooking Booking { get; set; }

        public RepetitionData Repetition { get; set; }
        public RepeatedSessionPricingData Pricing { get; set; }

        public IList<ApiOutOnlineBookingSingleSession> Sessions { get; set; }
    }
}