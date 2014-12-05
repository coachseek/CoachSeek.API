namespace CoachSeek.Domain.Services
{
    public class FortnightlySingleSessionListCalculator : SingleSessionListCalculator
    {
        protected override int OffsetNumberOfDays
        {
            get { return 14; }
        }
    }
}
