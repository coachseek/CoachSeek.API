using System.Collections.Generic;
using System.Linq;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Domain.Entities
{
    public class CourseBooking : Booking
    {
        public CourseKeyData Course { get; set; }
        public IList<SingleSessionBooking> SessionBookings { get; set; }


        public CourseBooking(BookingAddCommand command, RepeatedSessionData course)
            : base(command.Customer)
        {
            Course = new CourseKeyData { Id = course.Id };

            // Create session bookings in the course's session order.
            SessionBookings = new List<SingleSessionBooking>();
            foreach (var session in course.Sessions)
                if (command.Sessions.Select(x => x.Id).Contains(session.Id))
                    SessionBookings.Add(new SingleSessionBooking(new SessionKeyCommand(session.Id), command.Customer, Id));
        }


        private BookingAddCommand CreateSessionBookingAddCommand(BookingAddCommand courseCommand, SessionKeyCommand session)
        {
            return new BookingAddCommand
            {
                Customer = new CustomerKeyCommand {Id = courseCommand.Customer.Id},
                //Session = new SessionKeyCommand {Id = session.Id}
            };
        }
    }
}
