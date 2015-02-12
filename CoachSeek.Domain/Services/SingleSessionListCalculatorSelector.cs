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

            if (repeatFrequency == "w")
                return new WeeklySingleSessionListCalculator();

            return null;
        }

        public static ISingleSessionListCalculator SelectCalculator(RepeatFrequency repeatFrequency)
        {
            return SelectCalculator(repeatFrequency.Frequency);
        }
    }
}
