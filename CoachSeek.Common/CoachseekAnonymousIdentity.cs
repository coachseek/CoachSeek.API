using System;

namespace CoachSeek.Common
{
    public class CoachseekAnonymousIdentity : CoachseekIdentity
    {
        public CoachseekAnonymousIdentity(Guid businessId, string businessName)
            : base("anonymous", "none", businessId, businessName)
        { }
    }
}
