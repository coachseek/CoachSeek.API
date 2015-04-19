using System.Collections.Generic;

namespace CoachSeek.Api.Models.Api.Out
{
    public class ApiOutSessionBooking : ApiOutBooking
    {
        public List<ApiOutSessionCustomerBooking> Bookings { get; set; }
    }
}