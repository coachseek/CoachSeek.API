using System;
using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class SessionInvalid : SingleErrorException
    {
        public SessionInvalid(Guid sessionId)
            : base(ErrorCodes.SessionInvalid, 
                   "This session does not exist.",
                   sessionId.ToString())
        { }
    }
}
