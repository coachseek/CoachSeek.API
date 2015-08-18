using System;
using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class SessionCountInvalid : SingleErrorException
    {
        public SessionCountInvalid(int sessionCount)
            : base(ErrorCodes.SessionCountInvalid,
                   "The SessionCount field is not valid.",
                   sessionCount.ToString())
        { }
    }
}
