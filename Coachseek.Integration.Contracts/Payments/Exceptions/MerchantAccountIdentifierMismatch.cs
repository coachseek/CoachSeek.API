namespace Coachseek.Integration.Contracts.Payments.Exceptions
{
    public class MerchantAccountIdentifierMismatch : PaymentProcessingException
    {
        public override string Message
        {
            get { return "Merchant account identifiers don't match."; }
        }
    }
}
