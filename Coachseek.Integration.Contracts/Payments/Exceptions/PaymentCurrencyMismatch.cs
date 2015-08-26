namespace Coachseek.Integration.Contracts.Payments.Exceptions
{
    public class PaymentCurrencyMismatch : PaymentProcessingException
    {
        public override string Message
        {
            get { return "The payment currency does not match the business currency."; }
        }
    }
}
