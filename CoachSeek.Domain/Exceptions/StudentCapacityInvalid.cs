using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class StudentCapacityInvalid : SingleErrorException
    {
        public StudentCapacityInvalid(int studentCapacity)
            : base(ErrorCodes.StudentCapacityInvalid,
                   string.Format("StudentCapacity of {0} is not valid.", studentCapacity),
                   studentCapacity.ToString())
        { }
    }
}
