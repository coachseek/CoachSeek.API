using System;

namespace CoachSeek.Api.Models.Api.Out
{
    public class ApiOutSessionCustomerBooking : ApiOutCourseCustomerBooking
    {
        public Guid? ParentId { get; set; } // Course Booking Id if it's a session within a course.
        public bool? HasAttended { get; set; }
    }
}