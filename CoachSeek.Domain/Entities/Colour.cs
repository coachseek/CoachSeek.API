using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class Colour
    {
        public static string Default { get { return "green"; } }

        private readonly string _colour;

        public string Colouration { get { return _colour; } }

        public Colour(string colour)
        {
            if (colour == null)
                throw new InvalidColour();

            _colour = colour.Trim().ToLowerInvariant();

            Validate();
        }

        private void Validate()
        {
            if (!(IsYellow || IsOrange || IsGreen || IsMidGreen || IsDarkGreen ||
                  IsRed || IsDarkRed || IsBlue || IsMidBlue || IsDarkBlue))
                throw new InvalidColour();
        }

        private bool IsYellow { get { return Colouration == "yellow"; } }
        private bool IsOrange { get { return Colouration == "orange"; } }
        private bool IsGreen { get { return Colouration == "green"; } }
        private bool IsMidGreen { get { return Colouration == "mid-green"; } }
        private bool IsDarkGreen { get { return Colouration == "dark-green"; } }
        private bool IsRed { get { return Colouration == "red"; } }
        private bool IsDarkRed { get { return Colouration == "dark-red"; } }
        private bool IsBlue { get { return Colouration == "blue"; } }
        private bool IsMidBlue { get { return Colouration == "mid-blue"; } }
        private bool IsDarkBlue { get { return Colouration == "dark-blue"; } }
    }
}
