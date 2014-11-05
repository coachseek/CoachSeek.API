namespace CoachSeek.Domain.Commands
{
    public class ServicePricingCommand
    {
        public decimal? SessionPrice { get; set; }
        public decimal? CoursePrice { get; set; }
    }
}
