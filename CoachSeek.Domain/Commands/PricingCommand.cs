namespace CoachSeek.Domain.Commands
{
    public class PricingCommand
    {
        public decimal? SessionPrice { get; set; }
        public decimal? CoursePrice { get; set; }


        public PricingCommand()
        { }

        public PricingCommand(decimal? sessionPrice, decimal? coursePrice = null)
        {
            SessionPrice = sessionPrice;
            CoursePrice = coursePrice;
        }
    }
}
