using System;
using CoachSeek.Common;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Domain.Entities
{
    public class SingleSessionOnlineBooking : SingleSessionBooking
    {
        // Command parameters denote that it's data from outside the application (ie. user input).
        public SingleSessionOnlineBooking(SessionKeyCommand session, CustomerKeyCommand customer, Guid? parentId = null)
            : base(session, customer, parentId, Constants.PAYMENT_STATUS_PENDING_PAYMENT)
        { }
    }
}
