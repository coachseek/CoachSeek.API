namespace CoachSeek.Domain.Entities.Authentication
{
    public class CoachseekAnonymousIdentity : CoachseekIdentity
    {
        public CoachseekAnonymousIdentity(Business business)
            : base(new AnonymousUser(), business, "none")
        { }
    }
}
