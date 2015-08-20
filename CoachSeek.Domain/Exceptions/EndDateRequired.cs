using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class EndDateRequired : SingleErrorException
    {
        public EndDateRequired()
            : base(ErrorCodes.EndDateRequired,
                   "The EndDate field is required.")
        { }
    }
}
