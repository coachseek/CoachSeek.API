namespace Coachseek.Integration.Contracts.Exceptions
{
    public class PaymentAmountMismatch : PaymentProcessingException
    {
        public override string Message
        {
            get { return "The payment amount does not match the session or course amount."; }
        }
    }
}
