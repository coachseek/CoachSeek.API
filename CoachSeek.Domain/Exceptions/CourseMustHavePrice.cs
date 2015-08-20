using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class CourseMustHavePrice : SingleErrorException
    {
        public CourseMustHavePrice()
            : base(ErrorCodes.CourseMustHavePrice,
                   "A course must have at least a session or course price.")
        { }
    }
}