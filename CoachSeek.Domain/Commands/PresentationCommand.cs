namespace CoachSeek.Domain.Commands
{
    public class PresentationCommand
    {
        public string Colour { get; set; }

        public PresentationCommand()
        { }

        public PresentationCommand(string colour)
        {
            Colour = colour;
        }

    }
}
