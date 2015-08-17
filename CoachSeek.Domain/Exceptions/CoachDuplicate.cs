using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class CoachDuplicate : SingleErrorException
    {
        public CoachDuplicate(string coachName)
            : base(ErrorCodes.CoachDuplicate, 
                   string.Format("Coach '{0}' already exists.", coachName),
                   coachName)
        { }
    }
}