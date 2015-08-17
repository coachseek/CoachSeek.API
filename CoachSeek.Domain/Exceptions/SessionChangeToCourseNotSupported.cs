using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class SessionChangeToCourseNotSupported : SingleErrorException
    {
        public SessionChangeToCourseNotSupported()
            : base(ErrorCodes.SessionChangeToCourseNotSupported, "Cannot change a session to a course.")
        { }
    }
}
