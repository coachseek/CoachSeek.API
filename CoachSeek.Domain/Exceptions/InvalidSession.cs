using System;
using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class InvalidSession : SingleErrorException
    {
        public InvalidSession(Guid sessionId)
            : base(ErrorCodes.ServiceInvalid, 
                   "This session does not exist.",
                   sessionId.ToString())
        { }
    }
}
