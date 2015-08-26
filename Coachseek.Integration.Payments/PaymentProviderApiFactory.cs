using System;
using CoachSeek.Common;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;
using Coachseek.Integration.Contracts.Payments.Interfaces;

namespace Coachseek.Integration.Payments
{
    public class PaymentProviderApiFactory : IPaymentProviderApiFactory
    {
        public IPaymentProviderApi GetPaymentProviderApi(PaymentProcessingMessage message, bool isTestMessage)
        {
            if (message.PaymentProvider == Constants.PAYPAL)
                return new PaypalPaymentProviderApi(isTestMessage);

            throw new InvalidOperationException("Unknown payment provider.");
        }
    }
}
