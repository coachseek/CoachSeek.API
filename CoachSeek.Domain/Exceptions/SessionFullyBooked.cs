using System;
using CoachSeek.Common;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Domain.Exceptions
{
    public class SessionFullyBooked : SingleErrorException
    {
        public SessionFullyBooked(BookingSession session) 
            : this(session.Id)
        { }

        public SessionFullyBooked(Guid sessionId)
            : base(ErrorCodes.SessionFullyBooked,
                   "Session is already fully booked.",
                   sessionId.ToString())
        { }
    }
}
