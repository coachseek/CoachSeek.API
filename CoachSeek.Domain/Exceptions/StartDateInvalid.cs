using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class StartDateInvalid : DateInvalid
    {
        public StartDateInvalid(DateInvalid dateInvalid)
            : base(ErrorCodes.StartDateInvalid,
                   string.Format("'{0}' is not a valid start date.", dateInvalid.Date),
                   dateInvalid.Date)
        { }
    }
}
