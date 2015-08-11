using CoachSeek.Common;
using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Factories
{
    public static class PaymentProviderFactory
    {
        public static PaymentProviderBase CreateDefaultPaymentProvider()
        {
            return new NullPaymentProvider();
        }

        public static PaymentProviderBase CreatePaymentProvider(string providerType, string merchantAccountIdentifier)
        {
            if (providerType == null)
                return CreateDefaultPaymentProvider();
            if (providerType.CompareIgnoreCase(PaymentProvider.Test.ToString()))
                return new TestPaymentProvider(merchantAccountIdentifier);

            if (providerType.CompareIgnoreCase(PaymentProvider.PayPal.ToString()))
                return new PaypalPaymentProvider(merchantAccountIdentifier);

            throw new PaymentProviderNotSupported(providerType);
        }
    }
}
