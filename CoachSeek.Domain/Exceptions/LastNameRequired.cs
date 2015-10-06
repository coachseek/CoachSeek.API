using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class LastNameRequired : SingleErrorException
    {
        public LastNameRequired()
            : base(ErrorCodes.LastNameRequired, "The LastName field is required.")
        { }
    }
}
