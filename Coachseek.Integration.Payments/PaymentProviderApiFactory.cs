using System;
using CoachSeek.Common;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;
using Coachseek.Integration.Contracts.Interfaces;

namespace Coachseek.Integration.Payments
{
    public class PaymentProviderApiFactory : IPaymentProviderApiFactory
    {
        public IPaymentProviderApi GetPaymentProviderApi(PaymentProcessingMessage message, bool isPaymentEnabled)
        {
            if (message.PaymentProvider == Constants.PAYPAL)
                return new PaypalPaymentProviderApi(isPaymentEnabled);

            throw new InvalidOperationException("Unknown payment provider.");
        }
    }
}
