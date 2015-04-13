using System;

namespace CoachSeek.DataAccess.Main.Memory.Models
{
    public class DbBooking
    {
        public Guid Id { get; set; }
        public DbSessionKey Session { get; set; }
        public DbCustomerKey Customer { get; set; }
        public string PaymentStatus { get; set; }
        public bool? HasAttended { get; set; }
    }
}
