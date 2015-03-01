using System;
using CoachSeek.DataAccess.Models;

namespace CoachSeek.DataAccess.Main.Memory.Models
{
    public class DbSingleSession : DbSession
    {
        public Guid? ParentId { get; set; }
        public DbSingleSessionPricing Pricing { get; set; }
    }
}
