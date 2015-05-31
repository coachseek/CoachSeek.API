using System.Xml.Schema;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities.Payment
{
    public class PaypalPaymentProvider : PaymentProvider
    {
        public PaypalPaymentProvider(string merchantAccountIdentifier)
            : base(merchantAccountIdentifier)
        {
            if (MerchantAccountIdentifier == null)
                throw new MissingMerchantAccountIdentifier();

            Validation();
        }


        public override string Provider { get { return "PayPal"; } }


        private void Validation()
        {
            try
            {
                // PayPal merchant account identifiers are email addresses.
                var email = new EmailAddress(MerchantAccountIdentifier);
            }
            catch (InvalidEmailAddressFormat)
            {
                throw new InvalidMerchantAccountIdentifierFormat();
            }
        }
    }
}
