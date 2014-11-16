namespace CoachSeek.Domain.Commands
{
    public class ServiceBookingCommand
    {
        public int? StudentCapacity { get; set; }
        public bool? IsOnlineBookable { get; set; } // eg. Is private or not
    }
}
