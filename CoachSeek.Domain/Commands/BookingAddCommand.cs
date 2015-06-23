namespace CoachSeek.Domain.Commands
{
    public class BookingAddCommand
    {
        public SessionKeyCommand Session { get; set; }
        public CustomerKeyCommand Customer { get; set; }

        // When creating a booking we don't accept a payment status
        // and we don't accept the HasAttended flag. To do otherwise
        // would not make sense.

        //public string PaymentStatus { get; set; }
        //public bool? HasAttended { get; set; }
    }
}