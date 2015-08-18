using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class StandaloneSessionMustHaveNoCoursePrice : SingleErrorException
    {
        public StandaloneSessionMustHaveNoCoursePrice()
            : base(ErrorCodes.StandaloneSessionMustHaveNoCoursePrice,
                   "Standalone sessions must not have the CoursePrice set.")
        { }
    }
}
