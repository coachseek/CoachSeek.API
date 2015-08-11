using CoachSeek.Common;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Domain.Exceptions
{
    public class DuplicateUser : SingleErrorException
    {
        public DuplicateUser(User duplicateUser)
            : base(string.Format("The user with email address '{0}' already exists.", duplicateUser.Email),
                   ErrorCodes.UserDuplicate,
                   duplicateUser.Email)
        { }
    }
}
