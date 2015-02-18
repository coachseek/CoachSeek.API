namespace CoachSeek.Data.Model
{
    public class RepeatedSessionPricingData : SingleSessionPricingData
    {
        public decimal? CoursePrice { get; set; }


        public RepeatedSessionPricingData()
        { }

        public RepeatedSessionPricingData(decimal? sessionPrice, decimal? coursePrice = null)
            : base(sessionPrice)
        {
            CoursePrice = coursePrice;
        }
    }
}
