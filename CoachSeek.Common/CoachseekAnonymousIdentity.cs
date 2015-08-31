using System.Security.Principal;

namespace CoachSeek.Common
{
    public class CoachseekAnonymousIdentity : CoachseekIdentity
    {
        public CoachseekAnonymousIdentity(BusinessDetails business,
                                          CurrencyDetails currency)
            : base(new AnonymousUserDetails(), business, currency, "none")
        { }
    }
}
