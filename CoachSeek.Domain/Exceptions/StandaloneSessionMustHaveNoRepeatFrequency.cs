using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class StandaloneSessionMustHaveNoRepeatFrequency : SingleErrorException
    {
        public StandaloneSessionMustHaveNoRepeatFrequency()
            : base(ErrorCodes.StandaloneSessionMustHaveNoRepeatFrequency,
                   "Standalone sessions must not have the RepeatFrequency set.")
        { }
    }
}
