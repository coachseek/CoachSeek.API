using System;
using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class InvalidLocation : SingleErrorException
    {
        public InvalidLocation(Guid locationId)
            : base("This location does not exist.",
                   ErrorCodes.LocationInvalid,
                   locationId.ToString())
        { }
    }
}