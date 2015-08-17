using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class LocationDuplicate : SingleErrorException
    {
        public LocationDuplicate(string locationName)
            : base(ErrorCodes.LocationDuplicate, 
                   string.Format("Location '{0}' already exists.", locationName),
                   locationName)
        { }
    }
}