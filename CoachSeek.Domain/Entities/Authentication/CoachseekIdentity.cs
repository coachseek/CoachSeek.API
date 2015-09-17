using System.Security.Principal;
using CoachSeek.Common;
using CoachSeek.Domain.Contracts;

namespace CoachSeek.Domain.Entities.Authentication
{
    public class CoachseekIdentity : GenericIdentity
    {
        public IUser User { get; protected set; }
        public Business Business { get; protected set; }

        public CoachseekIdentity(IUser user,
                                 Business business,
                                 string authenticationType = "Basic")
            : base(user.Username, authenticationType)
        {
            User = user;
            Business = business;
        }
    }
}
