using System.Security.Principal;

namespace CoachSeek.Common
{
    public class CoachseekIdentity : GenericIdentity
    {
        public UserDetails User { get; protected set; }
        public BusinessDetails Business { get; protected set; }
        public CurrencyDetails Currency { get; protected set; }

        public CoachseekIdentity(UserDetails user,
                                 BusinessDetails business,
                                 CurrencyDetails currency,
                                 string authenticationType = "Basic")
            : base(user.Username, authenticationType)
        {
            User = user;
            Business = business;
            Currency = currency;
        }
    }
}
