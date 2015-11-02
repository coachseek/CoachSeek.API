namespace CoachSeek.Data.Model
{
    public class SingleSessionPricingData
    {
        public decimal? SessionPrice { get; set; }


        public SingleSessionPricingData(decimal? sessionPrice = null)
        {
            SessionPrice = sessionPrice;
        }
    }
}
