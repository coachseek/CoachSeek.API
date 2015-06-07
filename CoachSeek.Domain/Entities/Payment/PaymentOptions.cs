using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Factories;

namespace CoachSeek.Domain.Entities.Payment
{
    public class PaymentOptions
    {
        private readonly PaymentProvider _paymentProvider;

        public bool IsOnlinePaymentEnabled { get; protected set; }
        public bool ForceOnlinePayment { get; protected set; }
        public string Provider { get { return _paymentProvider.Provider; } }
        public string MerchantAccountIdentifier { get { return _paymentProvider.MerchantAccountIdentifier; } }


        protected PaymentOptions()
        {
            IsOnlinePaymentEnabled = false;
            ForceOnlinePayment = false;
            _paymentProvider = PaymentProviderFactory.CreateDefaultPaymentProvider();
        }

        public PaymentOptions(bool isOnlinePaymentEnabled, bool forceOnlinePayment, string paymentProvider, string merchantAccountIdentifier)
        {
            IsOnlinePaymentEnabled = isOnlinePaymentEnabled;
            ForceOnlinePayment = forceOnlinePayment;
            _paymentProvider = PaymentProviderFactory.CreatePaymentProvider(paymentProvider, merchantAccountIdentifier);

            Validate();
        }

        private void Validate()
        {
            if (IsOnlinePaymentEnabled && _paymentProvider is NullPaymentProvider)
                throw new ValidationException("When Online Payment is enabled then an Online Payment Provider must be specified.");
        }
    }
}
