namespace CoachSeek.Data.Model
{
    public class PricingData
    {
        public decimal? SessionPrice { get; set; }
        public decimal? CoursePrice { get; set; }


        public PricingData()
        { }

        public PricingData(decimal? sessionPrice, decimal? coursePrice)
        {
            SessionPrice = sessionPrice;
            CoursePrice = coursePrice;
        }
    }
}
