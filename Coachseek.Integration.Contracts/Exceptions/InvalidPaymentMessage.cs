namespace Coachseek.Integration.Contracts.Exceptions
{
    public class InvalidPaymentMessage : PaymentProcessingException
    {
        public override string Message
        {
            get { return "Invalid message."; }
        }
    }
}
