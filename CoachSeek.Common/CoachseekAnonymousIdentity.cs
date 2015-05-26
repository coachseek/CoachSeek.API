using System;

namespace CoachSeek.Common
{
    public class CoachseekAnonymousIdentity : CoachseekIdentity
    {
        public CoachseekAnonymousIdentity(Guid businessId, string businessName)
            : base(Constants.ANONYMOUS_USER, "none", businessId, businessName)
        { }
    }
}
