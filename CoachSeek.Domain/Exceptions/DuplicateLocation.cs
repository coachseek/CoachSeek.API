using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class DuplicateLocation : SingleErrorException
    {
        public DuplicateLocation(string locationName)
            : base(string.Format("Location '{0}' already exists.", locationName),
                   ErrorCodes.LocationDuplicate,
                   locationName)
        { }
    }
}