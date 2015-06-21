namespace Coachseek.Integration.Contracts.Exceptions
{
    public class InvalidBusiness : PaymentProcessingException
    {
        public override string Message
        {
            get { return "Invalid business."; }
        }
    }
}
