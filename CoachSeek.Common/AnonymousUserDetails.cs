using System;

namespace CoachSeek.Common
{
    public class AnonymousUserDetails : UserDetails
    {
        public AnonymousUserDetails()
            : base(Guid.Empty, Constants.ANONYMOUS_USER, string.Empty, string.Empty)
        { }
    }
}
