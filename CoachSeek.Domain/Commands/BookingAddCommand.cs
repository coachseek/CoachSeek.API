namespace CoachSeek.Domain.Commands
{
    public class BookingAddCommand
    {
        public SessionKeyCommand Session { get; set; }
        public CustomerKeyCommand Customer { get; set; }
    }
}