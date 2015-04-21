namespace CoachSeek.Api.Models.Api.Out
{
    public abstract class ApiOutBooking
    {
        public int StudentCapacity { get; set; }
        public int BookingCount { get; set; }
        public bool IsOnlineBookable { get; set; }
    }
}