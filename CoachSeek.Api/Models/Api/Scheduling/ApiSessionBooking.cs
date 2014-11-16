namespace CoachSeek.Api.Models.Api.Scheduling
{
    public class ApiSessionBooking
    {
        // StudentCapacity is not required because it may default to the Service value.
        public int? StudentCapacity { get; set; }
        // StudentCapacity is not required because it may default to the Service value.
        public bool? IsOnlineBookable { get; set; } // eg. Is private or not
    }
}