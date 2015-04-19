using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Api.Models.Api.Out
{
    public class ApiOutCourseCustomerBooking
    {
        public Guid Id { get; set; } // BookingId
        public CustomerData Customer { get; set; }
        public bool? HasAttended { get; set; }
        public string PaymentStatus { get; set; }
    }
}