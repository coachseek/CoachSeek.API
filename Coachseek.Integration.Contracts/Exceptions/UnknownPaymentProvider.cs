namespace Coachseek.Integration.Contracts.Exceptions
{
    public class UnknownPaymentProvider : PaymentProcessingException
    {
        public override string Message
        {
            get { return "Unexpected payment provider."; }
        }
    }
}
