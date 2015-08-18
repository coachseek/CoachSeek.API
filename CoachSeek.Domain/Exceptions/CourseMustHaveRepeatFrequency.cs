using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class CourseMustHaveRepeatFrequency : SingleErrorException
    {
        public CourseMustHaveRepeatFrequency()
            : base(ErrorCodes.CourseMustHaveRepeatFrequency,
                   "Courses must have the RepeatFrequency set.")
        { }
    }
}
