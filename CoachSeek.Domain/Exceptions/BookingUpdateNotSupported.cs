using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class BookingUpdateNotSupported : SingleErrorException
    {
        public BookingUpdateNotSupported()
            : base(ErrorCodes.BookingUpdateNotSupported, "Booking update is not yet supported.")
        { }
    }
}
