using System;

namespace CoachSeek.Data.Model
{
    public class CustomerBookingData
    {
        public Guid Id { get; set; } // BookingId
        public CustomerData Customer { get; set; }
    }
}
