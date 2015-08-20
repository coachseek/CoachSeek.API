using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class StartTimeInvalid : TimeInvalid
    {
        public StartTimeInvalid(TimeInvalid timeInvalid)
            : base(ErrorCodes.StartTimeInvalid,
                   string.Format("'{0}' is not a valid start time.", timeInvalid.Time),
                   timeInvalid.Time)
        { }
    }
}
