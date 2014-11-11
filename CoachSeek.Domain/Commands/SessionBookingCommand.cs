namespace CoachSeek.Domain.Commands
{
    public class SessionBookingCommand
    {
        public int? StudentCapacity { get; set; }
        public bool IsOnlineBookable { get; set; } // eg. Is private or not
    }
}
