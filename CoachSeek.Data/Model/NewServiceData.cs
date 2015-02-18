namespace CoachSeek.Data.Model
{
    public class NewServiceData : IData
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public ServiceTimingData Timing { get; set; }
        public ServiceBookingData Booking { get; set; }
        public RepeatedSessionPricingData Pricing { get; set; }
        public RepetitionData Repetition { get; set; }
        public PresentationData Presentation { get; set; }


        public string GetName()
        {
            return "service";
        }
    }
}
