using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class SessionDuplicate : SingleErrorException
    {
        public SessionDuplicate()
            : base(ErrorCodes.SessionDuplicate,
                   "Duplicate session.")
        { }
    }
}
