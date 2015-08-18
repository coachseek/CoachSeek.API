using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class ServiceForStandaloneSessionMustHaveNoCoursePrice : SingleErrorException
    {
        public ServiceForStandaloneSessionMustHaveNoCoursePrice()
            : base(ErrorCodes.ServiceForStandaloneSessionMustHaveNoCoursePrice,
                   "Services for standalone sessions must not have the CoursePrice set.")
        { }
    }
}
