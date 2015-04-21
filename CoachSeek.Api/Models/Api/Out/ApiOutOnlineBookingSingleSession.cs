using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Api.Models.Api.Out
{
    public class ApiOutOnlineBookingSingleSession : ApiOutSession
    {
        public Guid? ParentId { get; set; }

        public ApiOutOnlineBookingSessionBooking Booking { get; set; }
        public SingleSessionPricingData Pricing { get; set; }
        public SingleRepetitionData Repetition { get; set; }    // Have this so that in a response the client know it is a single session.
    }
}