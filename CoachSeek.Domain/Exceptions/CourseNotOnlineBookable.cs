using System;
using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class CourseNotOnlineBookable : SingleErrorException
    {
        public CourseNotOnlineBookable(Guid courseId)
            : base(ErrorCodes.CourseNotOnlineBookable,
                   "The course is not online bookable.",
                   courseId.ToString())
        { }
    }
}
