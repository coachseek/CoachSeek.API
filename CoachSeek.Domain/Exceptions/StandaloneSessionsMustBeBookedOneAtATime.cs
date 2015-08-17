using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class StandaloneSessionsMustBeBookedOneAtATime : SingleErrorException
    {
        public StandaloneSessionsMustBeBookedOneAtATime()
            : base(ErrorCodes.StandaloneSessionsMustBeBookedOneAtATime,
                   "Standalone sessions must be booked one at a time.")
        { }
    }
}
