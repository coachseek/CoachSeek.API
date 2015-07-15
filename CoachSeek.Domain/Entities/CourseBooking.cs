using System;
using System.Collections.Generic;
using System.Linq;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Domain.Entities
{
    public class CourseBooking : Booking
    {
        public RepeatedSessionData Course { get; private set; }
        public IList<SingleSessionBooking> SessionBookings { get; private set; }
        public decimal BookingPrice { get; private set; }


        public CourseBooking(BookingAddCommand command, RepeatedSessionData course)
            : base(command.Customer)
        {
            Course = course;
            CreateSessionBookings(command, course);
            CalculateBookingPrice();
        }


        private void CreateSessionBookings(BookingAddCommand command, RepeatedSessionData course)
        {
            // Create session bookings in the course's session order.
            SessionBookings = new List<SingleSessionBooking>();
            foreach (var session in course.Sessions)
                if (command.Sessions.Select(x => x.Id).Contains(session.Id))
                    SessionBookings.Add(new SingleSessionBooking(new SessionKeyCommand(session.Id), command.Customer, Id));
        }

        private void CalculateBookingPrice()
        {
            if (IsBookingForWholeCourse)
                BookingPrice = CalculateCoursePrice();
            else
                BookingPrice = CalculateMultiSessionPrice();
        }

        private bool IsBookingForWholeCourse
        {
            get { return SessionBookings.Count == Course.Sessions.Count; }
        }

        private decimal CalculateCoursePrice()
        {
            return Course.Pricing.CoursePrice ??
                   Course.Pricing.SessionPrice.Value * Course.Repetition.SessionCount;
        }

        private decimal CalculateMultiSessionPrice()
        {
            var sessionPrice = Course.Pricing.SessionPrice ??
                               Math.Round(Course.Pricing.CoursePrice.Value / Course.Repetition.SessionCount, 2);

            return sessionPrice * SessionBookings.Count;
        }
    }
}
