namespace Coachseek.Integration.Contracts.Payments.Exceptions
{
    public class InvalidBusiness : PaymentProcessingException
    {
        public override string Message
        {
            get { return "Invalid business."; }
        }
    }
}
