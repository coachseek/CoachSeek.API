using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class CourseChangeRepetitionNotSupported : SingleErrorException
    {
        public CourseChangeRepetitionNotSupported()
            : base(ErrorCodes.CourseChangeRepetitionNotSupported,
                   "Changing course repetition not supported.")
        { }
    }
}
