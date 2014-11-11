namespace CoachSeek.Data.Model
{
    public class NewSessionData : IData
    {
        public ServiceKeyData Service { get; set; }
        public LocationKeyData Location { get; set; }
        public CoachKeyData Coach { get; set; }

        //public SessionTimingData Timing { get; set; }
        //public SessionBookingData Booking { get; set; }
        public PricingData Pricing { get; set; }
        public RepetitionData Repetition { get; set; }
        public PresentationData Presentation { get; set; }


        public string GetName()
        {
            return "session";
        }

        public string GetBusinessIdPath()
        {
            return "session.businessId";
        }
    }
}
