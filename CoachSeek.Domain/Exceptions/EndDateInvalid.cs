using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class EndDateInvalid : DateInvalid
    {
        public EndDateInvalid(DateInvalid dateInvalid)
            : base(ErrorCodes.EndDateInvalid,
                   string.Format("'{0}' is not a valid end date.", dateInvalid.Date),
                   dateInvalid.Date)
        { }
    }
}
