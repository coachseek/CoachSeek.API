using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class InvalidColour : SingleErrorException
    {
        public InvalidColour(string colour)
            : base(string.Format("Colour '{0}' is not valid.", colour),
                   ErrorCodes.ColourInvalid,
                   colour)
        { }
    }
}
