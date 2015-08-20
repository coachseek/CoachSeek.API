using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class UserInvalid : SingleErrorException
    {
        public UserInvalid(string email)
            : base(ErrorCodes.UserInvalid,
                   string.Format("The user with email address '{0}' does not exist.", email),
                   email)
        { }
    }
}
