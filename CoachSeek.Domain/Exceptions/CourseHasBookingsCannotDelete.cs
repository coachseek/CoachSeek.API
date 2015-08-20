using System;
using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class CourseHasBookingsCannotDelete : SingleErrorException
    {
        public CourseHasBookingsCannotDelete(Guid courseId)
            : base(ErrorCodes.CourseHasBookingsCannotDelete,
                   "Cannot delete course as it has one or more bookings.",
                   courseId.ToString())
        { }
    }
}
