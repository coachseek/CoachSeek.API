using System.Collections.Generic;

namespace CoachSeek.Data.Model
{
    public class SessionBookingData
    {
        public int? StudentCapacity { get; set; }
        public int BookingCount { get; set; }
        public bool? IsOnlineBookable { get; set; } // eg. Is private or not
        public IList<CustomerBookingData> Bookings { get; set; }


        public SessionBookingData()
        {
            Bookings = new List<CustomerBookingData>();
        }

        public SessionBookingData(int? studentCapacity, bool? isOnlineBookable = null)
        {
            StudentCapacity = studentCapacity;
            IsOnlineBookable = isOnlineBookable;
            Bookings = new List<CustomerBookingData>();
        }
    }
}
