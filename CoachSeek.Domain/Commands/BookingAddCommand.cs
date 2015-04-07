namespace CoachSeek.Domain.Commands
{
    public class BookingAddCommand
    {
        public SessionKeyCommand Session { get; set; }
        public CustomerKeyCommand Customer { get; set; }
        public string PaymentStatus { get; set; }
        public bool? HasAttended { get; set; }
    }
}