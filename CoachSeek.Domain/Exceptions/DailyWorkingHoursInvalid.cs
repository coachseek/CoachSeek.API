using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class DailyWorkingHoursInvalid : SingleErrorException
    {
        public DailyWorkingHoursInvalid(string dayOfWeek)
            : base(ErrorCodes.DailyWorkingHoursInvalid,
                   string.Format("{0} working hours are not valid.", dayOfWeek),
                   dayOfWeek)
        { }
    }
}
