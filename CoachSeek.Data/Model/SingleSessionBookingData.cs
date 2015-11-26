using System;

namespace CoachSeek.Data.Model
{
    public class SingleSessionBookingData : BookingData
    {
        public Guid? ParentId { get; set; }
        public bool? HasAttended { get; set; }
        public bool? IsOnlineBooking { get; set; }
        public BookingSessionData Session { get; set; }
    }
}