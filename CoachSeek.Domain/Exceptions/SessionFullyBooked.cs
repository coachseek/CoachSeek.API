using System;
using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class SessionFullyBooked : SingleErrorException
    {
        public SessionFullyBooked(Guid sessionId)
            : base(ErrorCodes.SessionFullyBooked,
                   "This session is already fully booked.",
                   sessionId.ToString())
        { }
    }
}
