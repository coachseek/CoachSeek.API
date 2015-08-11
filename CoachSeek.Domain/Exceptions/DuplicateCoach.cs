using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class DuplicateCoach : SingleErrorException
    {
        public DuplicateCoach(string coachName)
            : base(string.Format("Coach '{0}' already exists.", coachName),
                   ErrorCodes.CoachDuplicate,
                   coachName)
        { }
    }
}