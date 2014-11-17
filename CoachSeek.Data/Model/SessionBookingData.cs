namespace CoachSeek.Data.Model
{
    public class SessionBookingData
    {
        public int? StudentCapacity { get; set; }
        public bool? IsOnlineBookable { get; set; } // eg. Is private or not


        public SessionBookingData()
        { }

        public SessionBookingData(int? studentCapacity, bool? isOnlineBookable = null)
        {
            StudentCapacity = studentCapacity;
            IsOnlineBookable = isOnlineBookable;
        }
    }
}
