using System;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Domain.Entities.Booking
{
    public class NewBooking : Booking
    {
        public NewBooking(SessionKeyData session, CustomerKeyData customer)
            : base(Guid.NewGuid(), session, customer)
        { }

        public NewBooking(SessionKeyCommand session, CustomerKeyCommand customer)
            : base(Guid.NewGuid(), session, customer)
        { }

        public NewBooking(BookingAddCommand command)
            : this(command.Session, command.Customer)
        { }
    }
}