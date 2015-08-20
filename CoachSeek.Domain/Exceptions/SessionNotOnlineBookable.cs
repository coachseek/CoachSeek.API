using System;
using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class SessionNotOnlineBookable : SingleErrorException
    {
        public SessionNotOnlineBookable(Guid sessionId)
            : base(ErrorCodes.SessionNotOnlineBookable,
                   "The session is not online bookable.",
                   sessionId.ToString())
        { }
    }
}
