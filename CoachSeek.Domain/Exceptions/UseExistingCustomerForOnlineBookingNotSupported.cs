using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class UseExistingCustomerForOnlineBookingNotSupported : SingleErrorException
    {
        public UseExistingCustomerForOnlineBookingNotSupported()
            : base(ErrorCodes.UseExistingCustomerForOnlineBookingNotSupported, "Using an existing customer for online booking is not supported.")
        { }
    }
}
