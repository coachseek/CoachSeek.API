using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class RepeatFrequencyInvalid : SingleErrorException
    {
        public RepeatFrequencyInvalid(string repeatFrequency)
            : base(ErrorCodes.RepeatFrequencyInvalid,
                   "The RepeatFrequency field is not valid.",
                   repeatFrequency)
        { }
    }
}
