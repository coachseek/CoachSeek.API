﻿using System;

namespace CoachSeek.Data.Model
{
    public abstract class BookingData
    {
        public Guid Id { get; set; }

        public CustomerKeyData Customer { get; set; }
        public string PaymentStatus { get; set; }
        public bool? HasAttended { get; set; }
    }
}