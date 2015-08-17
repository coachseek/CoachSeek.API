using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class ColourInvalid : SingleErrorException
    {
        public ColourInvalid(string colour)
            : base(ErrorCodes.ColourInvalid, 
                   string.Format("Colour '{0}' is not valid.", colour),
                   colour)
        { }
    }
}
