using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class Colour
    {
        protected const string DEFAULT_COLOUR = "green";

        private readonly string _colour;

        public virtual string Colouration { get { return _colour; } }

        public Colour(string colour)
        {
            if (string.IsNullOrEmpty(colour))
                _colour = DEFAULT_COLOUR;
            else
            {
                _colour = colour.Trim().ToLowerInvariant();
                Validate();
            }
        }

        private void Validate()
        {
            if (!(IsGreen || 
                  IsYellow || 
                  IsOrange || 
                  IsMidGreen || 
                  IsDarkGreen ||
                  IsRed || 
                  IsDarkRed || 
                  IsBlue || 
                  IsMidBlue || 
                  IsDarkBlue))
                throw new ColourInvalid(_colour);
        }

        private bool IsGreen { get { return _colour == "green"; } }
        private bool IsYellow { get { return _colour == "yellow"; } }
        private bool IsOrange { get { return _colour == "orange"; } }
        private bool IsMidGreen { get { return _colour == "mid-green"; } }
        private bool IsDarkGreen { get { return _colour == "dark-green"; } }
        private bool IsRed { get { return _colour == "red"; } }
        private bool IsDarkRed { get { return _colour == "dark-red"; } }
        private bool IsBlue { get { return _colour == "blue"; } }
        private bool IsMidBlue { get { return _colour == "mid-blue"; } }
        private bool IsDarkBlue { get { return _colour == "dark-blue"; } }
    }
}
