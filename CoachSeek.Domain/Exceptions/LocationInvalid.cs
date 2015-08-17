using System;
using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class LocationInvalid : SingleErrorException
    {
        public LocationInvalid(Guid locationId)
            : base(ErrorCodes.LocationInvalid, 
                   "This location does not exist.",
                   locationId.ToString())
        { }
    }
}