namespace CoachSeek.DataAccess.Models
{
    public class DbWeeklyWorkingHours
    {
        public DbDailyWorkingHours Monday { get; set; }
        public DbDailyWorkingHours Tuesday { get; set; }
        public DbDailyWorkingHours Wednesday { get; set; }
        public DbDailyWorkingHours Thursday { get; set; }
        public DbDailyWorkingHours Friday { get; set; }
        public DbDailyWorkingHours Saturday { get; set; }
        public DbDailyWorkingHours Sunday { get; set; }
    }
}
