namespace CoachSeek.Domain.Services
{
    public static class DurationFormatter
    {
        public static string Format(int duration)
        {
            var hours = CalculateHours(duration);
            var minutes = CalculateMinutes(duration, hours);

            if (hours > 0 && minutes == 0)
                return hours == 1 ? "1 hour" : string.Format("{0} hours", hours);

            if (hours > 0 && minutes > 0)
            {
                if (hours == 1)
                    return string.Format("1 hour and {0} minutes", minutes);
                return string.Format("{0} hours and {1} minutes", hours, minutes);
            }

            return string.Format("{0} minutes", minutes);
        }


        private static int CalculateHours(int duration)
        {
            return duration / 60;
        }

        private static int CalculateMinutes(int duration, int hours)
        {
            return duration - hours * 60;
        }
    }
}
