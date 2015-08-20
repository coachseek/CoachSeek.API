using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class BookingContainsDuplicateSessions : SingleErrorException
    {
        public BookingContainsDuplicateSessions()
            : base(ErrorCodes.BookingContainsDuplicateSessions,
                   "Booking contains duplicate sessions.")
        { }
    }
}
