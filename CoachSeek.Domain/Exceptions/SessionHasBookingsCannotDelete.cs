using System;
using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class SessionHasBookingsCannotDelete : SingleErrorException
    {
        public SessionHasBookingsCannotDelete(Guid sessionId)
            : base(ErrorCodes.SessionHasBookingsCannotDelete,
                   "Cannot delete session as it has one or more bookings.",
                   sessionId.ToString())
        { }
    }
}
