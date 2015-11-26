using System;
using CoachSeek.Common;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Entities
{
    public class SingleSessionOnlineBooking : SingleSessionBooking
    {
        public override bool IsOnlineBooking { get { return true; } }

        // Command parameters denote that it's data from outside the application (ie. user input).
        public SingleSessionOnlineBooking(BookingSession session, CustomerKeyData customer, Guid? parentId = null)
            : base(session, customer, parentId, Constants.PAYMENT_STATUS_PENDING_PAYMENT)
        { }

        public SingleSessionOnlineBooking(Guid id, 
                                          BookingSessionData session,
                                          CustomerKeyData customer, 
                                          string paymentStatus, 
                                          bool? hasAttended, 
                                          Guid? parentId = null)
            : base(id, session, customer, paymentStatus, hasAttended, parentId)
        { }
    }
}
