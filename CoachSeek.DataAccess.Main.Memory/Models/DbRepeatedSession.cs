using System.Collections.Generic;
using CoachSeek.DataAccess.Models;

namespace CoachSeek.DataAccess.Main.Memory.Models
{
    public class DbRepeatedSession : DbSession
    {
        public DbRepeatedSessionPricing Pricing { get; set; }
        public IList<DbSingleSession> Sessions { get; set; }
    }
}
