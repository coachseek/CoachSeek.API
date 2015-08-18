using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class StandaloneSessionMustHaveSessionPrice : SingleErrorException
    {
        public StandaloneSessionMustHaveSessionPrice()
            : base(ErrorCodes.StandaloneSessionMustHaveSessionPrice,
                   "Standalone sessions must have the SessionPrice set.")
        { }
    }
}
