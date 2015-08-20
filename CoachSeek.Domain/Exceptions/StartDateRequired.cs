using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class StartDateRequired : SingleErrorException
    {
        public StartDateRequired()
            : base(ErrorCodes.StartDateRequired,
                   "The StartDate field is required.")
        { }
    }
}
