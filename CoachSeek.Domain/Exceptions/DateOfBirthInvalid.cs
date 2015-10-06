using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class DateOfBirthInvalid : DateInvalid
    {
        public DateOfBirthInvalid(string date)
            : base(ErrorCodes.DateOfBirthInvalid,
                   string.Format("'{0}' is not a valid date of birth.", date),
                   date)
        { }
    }
}
