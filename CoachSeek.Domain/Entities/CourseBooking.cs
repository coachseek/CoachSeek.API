using System;
using System.Collections.Generic;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Domain.Entities
{
    public class CourseBooking : Booking
    {
        public SessionKeyData Course { get; set; }

        public IList<SingleSessionBooking> SessionBookings { get; set; }


        public CourseBooking(BookingAddCommand command, RepeatedSessionData existingCourse)
            : this(command.Session, command.Customer, existingCourse.Sessions)
        { }

        public CourseBooking(SessionKeyCommand course, CustomerKeyCommand customer, IList<SingleSessionData> existingSessions)
            : base(customer)
        {
            Course = new SessionKeyData { Id = course.Id };

            SessionBookings = new List<SingleSessionBooking>();
            var customerKeyData = new CustomerKeyData { Id = customer.Id };
            foreach (var session in existingSessions)
                SessionBookings.Add(new SingleSessionBooking(Guid.NewGuid(), session.ToKeyData(), customerKeyData, Id));
        }

        public CourseBooking(Guid id, SessionKeyData course, CustomerKeyData customer)
            : base(id, customer)
        {
            Course = new SessionKeyData { Id = course.Id };                        
        }
    }
}
