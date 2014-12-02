namespace CoachSeek.Domain.Services
{
    public class WeeklySingleSessionListCalculator : SingleSessionListCalculator
    {
        protected override int OffsetNumberOfDays
        {
            get { return 7; }
        }
    }
}
