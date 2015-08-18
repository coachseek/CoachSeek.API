using CoachSeek.Common;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class PaypalPaymentProvider : PaymentProviderBase
    {
        public PaypalPaymentProvider(string merchantAccountIdentifier)
            : base(merchantAccountIdentifier)
        {
            Validation();
        }


        public override string ProviderName { get { return PaymentProvider.PayPal.ToString(); } }


        private void Validation()
        {
            if (MerchantAccountIdentifier == null)
                throw new MerchantAccountIdentifierRequired();

            try
            {
                // PayPal merchant account identifiers are email addresses.
                var email = new EmailAddress(MerchantAccountIdentifier);
            }
            catch (EmailAddressFormatInvalid)
            {
                throw new MerchantAccountIdentifierFormatInvalid();
            }
        }
    }
}
