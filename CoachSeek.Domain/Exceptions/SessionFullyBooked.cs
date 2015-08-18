using System;
using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class SessionFullyBooked : SingleErrorException
    {
        public SessionFullyBooked(Guid sessionId)
            : base(ErrorCodes.SessionFullyBooked,
                   "Session is already fully booked.",
                   sessionId.ToString())
        { }
    }
}
