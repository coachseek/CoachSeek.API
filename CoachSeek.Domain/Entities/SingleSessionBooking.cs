using System;
using CoachSeek.Common;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Domain.Entities
{
    public class SingleSessionBooking : Booking
    {
        public Guid? ParentId { get; set; }
        public bool HasAttended { get; set; }
        public SessionKeyData Session { get; set; }

        // Command parameters denote that it's data from outside the application (ie. user input).
        public SingleSessionBooking(SessionKeyCommand session, CustomerKeyCommand customer, Guid? parentId = null)
            : base(customer)
        {
            ParentId = parentId;
            HasAttended = false;
            Session = new SessionKeyData { Id = session.Id };
        }

        // Data parameters denote that it's data from inside the application (ie. database).
        public SingleSessionBooking(Guid id, 
                                    SessionKeyData session,
                                    CustomerKeyData customer, 
                                    string paymentStatus = Constants.PAYMENT_STATUS_PENDING_INVOICE, 
                                    bool hasAttended = false, 
                                    Guid? parentId = null)
            : base(id, paymentStatus, customer)
        {
            ParentId = parentId;
            HasAttended = hasAttended;
            Session = new SessionKeyData { Id = session.Id };
        }
    }
}
