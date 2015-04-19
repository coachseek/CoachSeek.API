using System.Collections.Generic;
using CoachSeek.Data.Model;

namespace CoachSeek.Api.Models.Api.Out
{
    public class ApiOutCourse : ApiOutSession
    {
        public ApiOutCourseBooking Booking { get; set; }

        public RepetitionData Repetition { get; set; }
        public RepeatedSessionPricingData Pricing { get; set; }

        public IList<ApiOutSingleSession> Sessions { get; set; }
    }
}