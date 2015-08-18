using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class CourseExceedsMaximumNumberOfDailySessions : SingleErrorException
    {
        public CourseExceedsMaximumNumberOfDailySessions(int sessionCount, int maxSessionCount)
            : base(ErrorCodes.CourseExceedsMaximumNumberOfDailySessions,
                   string.Format("{0} exceeds the maximum number of daily sessions in a course of {1}.", sessionCount, maxSessionCount),
                   string.Format("Maximum Allowed Daily Session Count: {0}; Specified Session Count: {1}", maxSessionCount, sessionCount))
        { }
    }
}
