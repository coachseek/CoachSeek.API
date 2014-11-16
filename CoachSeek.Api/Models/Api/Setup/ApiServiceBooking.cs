namespace CoachSeek.Api.Models.Api.Setup
{
    public class ApiServiceBooking
    {
        public int? StudentCapacity { get; set; }
        public bool? IsOnlineBookable { get; set; } // eg. Is private or not
    }
}