using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class Colour
    {
        private readonly string _colour;

        public string Colouration { get { return _colour; } }

        public Colour(string colour)
        {
            if (colour == null)
                return;

            _colour = colour.Trim().ToLowerInvariant();

            Validate();
        }

        private void Validate()
        {
            if (!(IsRed || IsBlue || IsGreen || IsYellow || IsOrange))
                throw new InvalidColour();
        }

        private bool IsRed { get { return Colouration == "red"; } }
        private bool IsBlue { get { return Colouration == "blue"; } }
        private bool IsGreen { get { return Colouration == "green"; } }
        private bool IsYellow { get { return Colouration == "yellow"; } }
        private bool IsOrange { get { return Colouration == "orange"; } }
    }
}
