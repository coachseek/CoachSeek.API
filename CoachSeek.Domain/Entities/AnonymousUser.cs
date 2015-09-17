using CoachSeek.Common;
using CoachSeek.Domain.Contracts;

namespace CoachSeek.Domain.Entities
{
    public class AnonymousUser : IUser
    {
        public string Username { get { return Constants.ANONYMOUS_USER; }}
    }
}
