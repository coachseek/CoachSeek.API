using System;

namespace CoachSeek.Common
{
    public class CoachseekAnonymousIdentity : CoachseekIdentity
    {
        public CoachseekAnonymousIdentity(Guid businessId)
            : base("anonymous", "none", businessId)
        { }
    }
}
