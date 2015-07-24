using System;
using CoachSeek.Common;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Domain.Entities
{
    public class SingleSessionOnlineBooking : SingleSessionBooking
    {
        // Command parameters denote that it's data from outside the application (ie. user input).
        public SingleSessionOnlineBooking(SessionKeyCommand session, CustomerKeyCommand customer, Guid? parentId = null)
            : base(session, customer, parentId, Constants.PAYMENT_STATUS_PENDING_PAYMENT)
        { }


        // Data parameters denote that it's data from inside the application (ie. database).
        public SingleSessionOnlineBooking(Guid id, 
                                          SessionKeyData session,
                                          CustomerKeyData customer, 
                                          string paymentStatus = Constants.PAYMENT_STATUS_PENDING_PAYMENT, 
                                          bool hasAttended = false, 
                                          Guid? parentId = null)
            : base(id, session, customer, paymentStatus, hasAttended, parentId)
        { }
    }
}
