using System;
using System.Web;
using CoachSeek.Common;
using CoachSeek.Domain.Entities;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;

namespace Coachseek.Integration.Payments.PaymentsProcessor
{
    public static class PaymentConverter
    {
        public static Payment Convert(PaymentProcessingMessage message)
        {
            if (message.Contents == Constants.PAYPAL)
                return ConvertFromPaypal(message.Contents);

            throw new InvalidOperationException("Unexpected payment provider.");
        }

        public static Payment ConvertFromPaypal(string paypalMessage)
        {
            var message = HttpUtility.ParseQueryString(paypalMessage);


            return new Payment();
        }
    }
}
