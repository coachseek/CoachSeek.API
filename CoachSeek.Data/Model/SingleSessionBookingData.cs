﻿using System;

namespace CoachSeek.Data.Model
{
    public class SingleSessionBookingData : BookingData
    {
        public Guid? ParentId { get; set; }

        public SessionKeyData Session { get; set; }
    }
}