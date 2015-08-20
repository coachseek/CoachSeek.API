using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class TimeInvalid : SingleErrorException
    {
        public string Time { get; private set; }


        public TimeInvalid(string time)
            : base(ErrorCodes.TimeInvalid,
                   string.Format("'{0}' is not a valid time.", time),
                   time)
        {
            Time = time;
        }

        protected TimeInvalid(string errorCode, string message, string time)
            : base(errorCode,
                   message,
                   time)
        {
            Time = time;
        }
    }
}
