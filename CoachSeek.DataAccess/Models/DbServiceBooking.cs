namespace CoachSeek.DataAccess.Models
{
    public class DbServiceBooking
    {
        public int? StudentCapacity { get; set; }
        public bool? IsOnlineBookable { get; set; } // eg. Is private or not
    }
}
