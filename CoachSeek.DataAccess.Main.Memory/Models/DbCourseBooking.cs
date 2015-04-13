using System.Collections.Generic;

namespace CoachSeek.DataAccess.Main.Memory.Models
{
    public class DbCourseBooking : DbBooking
    {
        public IList<DbSingleSessionBooking> SessionBookings { get; set; }
    }
}
