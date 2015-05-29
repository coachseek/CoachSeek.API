namespace CoachSeek.Common
{
    public class CoachseekAnonymousIdentity : CoachseekIdentity
    {
        public CoachseekAnonymousIdentity(BusinessDetails business, CurrencyDetails currency)
            : base(Constants.ANONYMOUS_USER, "none", business, currency)
        { }
    }
}
