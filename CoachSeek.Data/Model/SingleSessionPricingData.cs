namespace CoachSeek.Data.Model
{
    public class SingleSessionPricingData
    {
        public decimal? SessionPrice { get; set; }


        public SingleSessionPricingData()
        { }

        public SingleSessionPricingData(decimal? sessionPrice)
        {
            SessionPrice = sessionPrice;
        }
    }
}
