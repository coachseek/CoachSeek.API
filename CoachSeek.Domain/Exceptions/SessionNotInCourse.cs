using System;
using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class SessionNotInCourse : SingleErrorException
    {
        public SessionNotInCourse(Guid sessionId, Guid courseId)
            : base(ErrorCodes.SessionNotInCourse,
                   "Session is not in course.", 
                   string.Format("Session: '{0}', Course: '{1}'", sessionId, courseId))
        { }
    }
}
