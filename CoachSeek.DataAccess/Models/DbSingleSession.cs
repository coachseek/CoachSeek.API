using System;

namespace CoachSeek.DataAccess.Models
{
    public class DbSingleSession : DbSession
    {
        public DbSingleSessionPricing Pricing { get; set; }
    }
}
