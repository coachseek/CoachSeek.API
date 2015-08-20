using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class DateInvalid : SingleErrorException
    {
        public string Date { get; private set; }


        public DateInvalid(string date)
            : base(ErrorCodes.DateInvalid,
                   string.Format("'{0}' is not a valid date.", date),
                   date)
        {
            Date = date;
        }

        protected DateInvalid(string errorCode, string message, string date)
            : base(errorCode,
                   message,
                   date)
        {
            Date = date;
        }
    }
}
