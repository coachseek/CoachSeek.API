namespace Coachseek.Integration.Contracts.Payments.Exceptions
{
    public class InvalidPaymentMessage : PaymentProcessingException
    {
        public override string Message
        {
            get { return "Invalid message."; }
        }
    }
}
