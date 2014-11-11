namespace CoachSeek.Data.Model
{
    public class NewServiceData : IData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ServiceDefaultsData Defaults { get; set; }
        public PricingData Pricing { get; set; }
        public RepetitionData Repetition { get; set; }


        public string GetName()
        {
            return "service";
        }

        public string GetBusinessIdPath()
        {
            return "service.businessId";
        }
    }
}
