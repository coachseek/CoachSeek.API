using System.Collections.Generic;

namespace CoachSeek.DataAccess.Models
{
    public class DbRepeatedSession : DbSession
    {
        public DbRepeatedSessionPricing Pricing { get; set; }
        public IList<DbSingleSession> Sessions { get; set; }
    }
}
