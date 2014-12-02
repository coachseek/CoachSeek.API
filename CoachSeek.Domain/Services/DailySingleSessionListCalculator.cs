namespace CoachSeek.Domain.Services
{
    public class DailySingleSessionListCalculator : SingleSessionListCalculator
    {
        protected override int OffsetNumberOfDays
        {
            get { return 1; }
        }
    }
}
