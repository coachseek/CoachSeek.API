using CoachSeek.Domain.Contracts;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Domain.Services
{
    public static class SingleSessionListCalculatorSelector
    {
        public static ISingleSessionListCalculator SelectCalculator(RepeatFrequency repeatFrequency)
        {
            if (repeatFrequency.Frequency == "d")
                return new DailySingleSessionListCalculator();

            return null;
        }
    }
}
