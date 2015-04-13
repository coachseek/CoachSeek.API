using System;

namespace CoachSeek.DataAccess.Main.Memory.Models
{
    public class DbSingleSessionBooking : DbBooking
    {
        public Guid? ParentId { get; set; }
    }
}
