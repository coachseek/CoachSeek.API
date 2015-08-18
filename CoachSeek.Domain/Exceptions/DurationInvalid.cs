using System;
using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class DurationInvalid : SingleErrorException
    {
        public DurationInvalid(int duration)
            : base(ErrorCodes.DurationInvalid,
                   string.Format("Duration '{0}' is not valid.", duration),
                   duration.ToString())
        { }
    }
}
