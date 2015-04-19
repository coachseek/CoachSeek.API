namespace CoachSeek.Domain.Commands
{
    public class SessionAddCommand
    {
        public ServiceKeyCommand Service { get; set; }
        public LocationKeyCommand Location { get; set; }
        public CoachKeyCommand Coach { get; set; }

        public SessionTimingCommand Timing { get; set; }
        public SessionBookingCommand Booking { get; set; }
        public PricingCommand Pricing { get; set; }
        public RepetitionCommand Repetition { get; set; }
        public PresentationCommand Presentation { get; set; }
    }
}
