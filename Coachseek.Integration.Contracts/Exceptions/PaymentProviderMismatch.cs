namespace Coachseek.Integration.Contracts.Exceptions
{
    public class PaymentProviderMismatch : PaymentProcessingException
    {
        public override string Message
        {
            get { return "Payment Providers don't match."; }
        }
    }
}
