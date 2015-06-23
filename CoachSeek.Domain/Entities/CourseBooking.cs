using System.Collections.Generic;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Domain.Entities
{
    public class CourseBooking : Booking
    {
        public SessionKeyData Course { get; set; }
        public IList<SingleSessionBooking> SessionBookings { get; set; }


        public CourseBooking(BookingAddCommand command, IEnumerable<SessionInCourse> existingSessions)
            : base(command)
        {
            Course = new SessionKeyData { Id = command.Session.Id };

            SessionBookings = new List<SingleSessionBooking>();
            foreach (var session in existingSessions)
            {
                var sessionCommand = CreateSessionBookingAddCommand(command, session);
                SessionBookings.Add(new SingleSessionBooking(sessionCommand, Id));                
            }
        }


        private BookingAddCommand CreateSessionBookingAddCommand(BookingAddCommand courseCommand, SessionInCourse session)
        {
            return new BookingAddCommand
            {
                Customer = new CustomerKeyCommand {Id = courseCommand.Customer.Id},
                Session = new SessionKeyCommand {Id = session.Id}
            };
        }
    }
}
