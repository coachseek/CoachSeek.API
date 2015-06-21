namespace Coachseek.Integration.Contracts.Exceptions
{
    public class InvalidBooking : PaymentProcessingException
    {
        public override string Message
        {
            get { return "Invalid booking."; }
        }
    }
}
