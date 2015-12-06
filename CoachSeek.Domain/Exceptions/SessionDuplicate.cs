using System;
using System.Collections.Generic;
using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class SessionDuplicate : SingleErrorException
    {
        public SessionDuplicate(IEnumerable<Guid> sessionIds)
            : base(ErrorCodes.SessionDuplicate,
                   "Duplicate session.",
                   ConcatenateSessionIds(sessionIds))
        { }

        private static string ConcatenateSessionIds(IEnumerable<Guid> sessionIds)
        {
            var output = string.Empty;
            foreach (var sessionId in sessionIds)
                output += string.Format("{0},", sessionId);
            return output.TrimEnd(',').ToLowerInvariant();
        }
    }
}
