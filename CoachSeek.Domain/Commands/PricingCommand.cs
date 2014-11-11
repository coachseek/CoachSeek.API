namespace CoachSeek.Domain.Commands
{
    public class PricingCommand
    {
        public decimal? SessionPrice { get; set; }
        public decimal? CoursePrice { get; set; }
    }
}
