using System;
using CoachSeek.Common;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;
using Coachseek.Integration.Contracts.Interfaces;

namespace Coachseek.Integration.Payments
{
    public static class PaymentProviderApiFactory
    {
        public static IPaymentsProviderApi GetPaymentProviderApi(PaymentProcessingMessage message, bool isPaymentEnabled)
        {
            if (message.PaymentProvider == Constants.PAYPAL)
                return new PaypalPaymentsProviderApi(isPaymentEnabled);

            throw new InvalidOperationException("Unknown payment provider.");
        }
    }
}
