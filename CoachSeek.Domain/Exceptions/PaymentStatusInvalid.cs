using System;
using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class PaymentStatusInvalid : SingleErrorException
    {
        public PaymentStatusInvalid(string paymentStatus)
            : base(ErrorCodes.PaymentStatusInvalid,
                   string.Format("Payment status '{0}' does not exist.", paymentStatus),
                   paymentStatus)
        { }
    }
}
