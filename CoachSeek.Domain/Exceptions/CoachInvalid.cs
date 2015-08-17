using System;
using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class CoachInvalid : SingleErrorException
    {
        public CoachInvalid(Guid coachId)
            : base(ErrorCodes.CoachInvalid, 
                   "This coach does not exist.",
                   coachId.ToString())
        { }
    }
}