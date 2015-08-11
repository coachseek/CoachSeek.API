using System;
using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class InvalidCoach : SingleErrorException
    {
        public InvalidCoach(Guid coachId)
            : base("This coach does not exist.",
                   ErrorCodes.CoachInvalid,
                   coachId.ToString())
        { }
    }
}