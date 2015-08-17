using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class BookingSessionRequired : SingleErrorException
    {
        public BookingSessionRequired()
            : base(ErrorCodes.BookingSessionRequired, "A booking must have at least one session.")
        { }
    }
}
