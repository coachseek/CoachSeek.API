namespace CoachSeek.DataAccess.Models
{
    public class DbRepeatedSessionPricing : DbSingleSessionPricing
    {
        public decimal? CoursePrice { get; set; }
    }
}
