using CoachSeek.Common;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Domain.Exceptions
{
    public class UserDuplicate : SingleErrorException
    {
        public UserDuplicate(User duplicateUser)
            : base(ErrorCodes.UserDuplicate, 
                   string.Format("The user with email address '{0}' already exists.", duplicateUser.Email),
                   duplicateUser.Email)
        { }
    }
}
