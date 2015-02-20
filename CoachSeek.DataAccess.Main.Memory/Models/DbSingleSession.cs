using System;

namespace CoachSeek.DataAccess.Models
{
    public class DbSingleSession : DbSession
    {
        public Guid? ParentId { get; set; }
        public DbSingleSessionPricing Pricing { get; set; }
    }
}
