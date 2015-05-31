using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Entities.Payment;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Factories
{
    public static class PaymentProviderFactory
    {
        public static PaymentProvider CreateDefaultPaymentProvider()
        {
            return new NullPaymentProvider();
        }

        public static PaymentProvider CreatePaymentProvider(string providerType, string merchantAccountIdentifier)
        {
            if (providerType == null)
                return CreateDefaultPaymentProvider();

            if (providerType.ToLower() == "paypal")
                return new PaypalPaymentProvider(merchantAccountIdentifier);

            throw new PaymentProviderNotSupported();
        }
    }
}
