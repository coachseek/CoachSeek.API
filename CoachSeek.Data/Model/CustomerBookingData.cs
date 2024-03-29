﻿using System;

namespace CoachSeek.Data.Model
{
    public class CustomerBookingData
    {
        public Guid Id { get; set; } // BookingId
        public Guid? ParentId { get; set; } // Course Booking Id
        public Guid SessionId { get; set; }
        public CustomerData Customer { get; set; }
        public bool? HasAttended { get; set; }
        public string PaymentStatus { get; set; }
        public bool IsOnlineBooking { get; set; }
        public int DiscountPercent { get; set; }
    }
}
