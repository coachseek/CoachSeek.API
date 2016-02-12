using CoachSeek.Common;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class PaypalPaymentProvider : PaymentProviderBase
    {
        public PaypalPaymentProvider(bool isOnlinePaymentEnabled, string merchantAccountIdentifier)
            : base(isOnlinePaymentEnabled, merchantAccountIdentifier)
        {
            Validation();
        }


        public override string ProviderName { get { return PaymentProvider.PayPal.ToString(); } }


        private void Validation()
        {
            if (IsOnlinePaymentEnabled && MerchantAccountIdentifier == null)
                throw new MerchantAccountIdentifierRequired();

            try
            {
                // PayPal merchant account identifiers are email addresses.
                var email = new EmailAddress(MerchantAccountIdentifier);
            }
            catch (EmailAddressRequired)
            {
                if (IsOnlinePaymentEnabled)
                    throw;
            }
            catch (EmailAddressFormatInvalid)
            {
                throw new MerchantAccountIdentifierFormatInvalid();
            }
        }
    }
}
