using System;
using System.Collections.Generic;
using System.Linq;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Domain.Entities
{
    public class CourseBooking : Booking
    {
        public RepeatedSessionData Course { get; protected set; }
        public IList<SingleSessionBooking> SessionBookings { get; protected set; }
        public decimal BookingPrice { get; protected set; }


        public CourseBooking(BookingAddCommand command, RepeatedSessionData course)
            : base(command.Customer)
        {
            Course = course;
            CreateSessionBookings(command, course);
            CalculateBookingPrice();
        }


        protected void CreateSessionBookings(BookingAddCommand command, RepeatedSessionData course)
        {
            // Create session bookings in the course's session order.
            SessionBookings = new List<SingleSessionBooking>();
            foreach (var session in course.Sessions)
                if (command.Sessions.Select(x => x.Id).Contains(session.Id))
                    SessionBookings.Add(CreateSingleSessionBooking(session, command.Customer, Id));
        }

        protected virtual SingleSessionBooking CreateSingleSessionBooking(SingleSessionData session, CustomerKeyCommand customer, Guid parentId)
        {
            return new SingleSessionBooking(new SessionKeyCommand(session.Id), customer, parentId);
        }

        protected void CalculateBookingPrice()
        {
            BookingPrice = IsBookingForWholeCourse ? CalculateCoursePrice() : CalculateMultiSessionPrice();
        }

        private bool IsBookingForWholeCourse
        {
            get { return SessionBookings.Count == Course.Sessions.Count; }
        }

        private decimal CalculateCoursePrice()
        {
            return Course.Pricing.CoursePrice ??
                   Course.Pricing.SessionPrice.GetValueOrDefault() * Course.Repetition.SessionCount;
        }

        private decimal CalculateMultiSessionPrice()
        {
            var sessionPrice = Course.Pricing.SessionPrice ??
                               Math.Round(Course.Pricing.CoursePrice.GetValueOrDefault() / Course.Repetition.SessionCount, 2);

            return sessionPrice * SessionBookings.Count;
        }
    }
}
