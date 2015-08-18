using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class StandaloneSessionMustBeBookedOneAtATime : SingleErrorException
    {
        public StandaloneSessionMustBeBookedOneAtATime()
            : base(ErrorCodes.StandaloneSessionMustBeBookedOneAtATime,
                   "Standalone sessions must be booked one at a time.")
        { }
    }
}
