using System;

namespace CoachSeek.Data.Model
{
    public class CustomerBookingData
    {
        public Guid BookingId { get; set; }
        public CustomerData Customer { get; set; }
    }
}
