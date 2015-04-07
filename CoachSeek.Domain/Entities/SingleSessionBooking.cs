using System;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Domain.Entities
{
    public class SingleSessionBooking : Booking
    {
        public Guid? ParentId { get; set; }
        public SessionKeyData Session { get; set; }


        public SingleSessionBooking(BookingAddCommand command)
            : this(command.Session, command.Customer)
        {
            PaymentStatus = command.PaymentStatus;
            HasAttended = command.HasAttended;
        }

        public SingleSessionBooking(SessionKeyCommand session, CustomerKeyCommand customer)
            : base(customer)
        {
            ParentId = null;
            Session = new SessionKeyData { Id = session.Id };
        }

        public SingleSessionBooking(Guid id, SessionKeyData session, CustomerKeyData customer, Guid? parentId = null)
            : base(id, customer)
        {
            ParentId = parentId;
            Session = new SessionKeyData { Id = session.Id };
        }
    }
}
