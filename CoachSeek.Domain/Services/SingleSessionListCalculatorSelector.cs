using CoachSeek.Domain.Contracts;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Domain.Services
{
    public static class SingleSessionListCalculatorSelector
    {
        public static ISingleSessionListCalculator SelectCalculator(string repeatFrequency)
        {
            if (repeatFrequency == "d")
                return new DailySingleSessionListCalculator();

            if (repeatFrequency == "2d")
                return new SecondDaySingleSessionListCalculator();

            if (repeatFrequency == "w")
                return new WeeklySingleSessionListCalculator();

            if (repeatFrequency == "2w")
                return new FortnightlySingleSessionListCalculator();

            return null;
        }

        public static ISingleSessionListCalculator SelectCalculator(RepeatFrequency repeatFrequency)
        {
            return SelectCalculator(repeatFrequency.Frequency);
        }
    }
}
