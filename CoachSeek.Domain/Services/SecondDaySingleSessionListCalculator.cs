namespace CoachSeek.Domain.Services
{
    public class SecondDaySingleSessionListCalculator : SingleSessionListCalculator
    {
        protected override int OffsetNumberOfDays
        {
            get { return 2; }
        }
    }
}
