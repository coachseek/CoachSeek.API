using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class CourseExceedsMaximumNumberOfWeeklySessions : SingleErrorException
    {
        public CourseExceedsMaximumNumberOfWeeklySessions(int sessionCount, int maxSessionCount)
            : base(ErrorCodes.CourseExceedsMaximumNumberOfWeeklySessions,
                   string.Format("{0} exceeds the maximum number of weekly sessions in a course of {1}.", sessionCount, maxSessionCount),
                   string.Format("Maximum Allowed Weekly Session Count: {0}; Specified Session Count: {1}", maxSessionCount, sessionCount))
        { }
    }
}
