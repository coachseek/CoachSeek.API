using System;
using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class CustomerAlreadyBookedOntoSession : SingleErrorException
    {
        public CustomerAlreadyBookedOntoSession(Guid customerId, Guid sessionId)
            : base(ErrorCodes.CustomerAlreadyBookedOntoSession,
                   "This customer is already booked for this session.",
                   string.Format("Customer: '{0}', Session: '{1}'", customerId, sessionId))
        { }
    }
}
