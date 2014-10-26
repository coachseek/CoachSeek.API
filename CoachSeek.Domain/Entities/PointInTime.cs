using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Exceptions;
using System;

namespace CoachSeek.Domain.Entities
{
    public class PointInTime
    {
        public int Hour { get; private set; }
        public int Minute { get; private set; }


        public PointInTime(string time)
        {
            if (time == null)
                throw new InvalidPointInTime();

            var components = SplitIntoHourAndMinute(time);
            Hour = ParseOutHour(components[0]);
            Minute = ParseOutMinute(components[1]);
        }


        public bool IsAfter(PointInTime earlier)
        {
            if (earlier.Hour > Hour)
                return false;

            if (earlier.Hour == Hour && earlier.Minute >= Minute)
                    return false;                

            return true;
        }

        public string ToData()
        {
            return string.Format("{0}:{1:D2}", Hour, Minute);
        }


        private static string[] SplitIntoHourAndMinute(string time)
        {
            var components = time.Trim().Split(':');
            if (components.GetLength(0) != 2)
                throw new InvalidPointInTime();
            return components;
        }

        private int ParseOutHour(string hourString)
        {
            if (hourString.Length < 1 || hourString.Length > 2)
                throw new InvalidPointInTime();

            int hourInt;
            try
            {
                hourInt = hourString.ParseOrThrow<int>();
            }
            catch (Exception)
            {
                throw new InvalidPointInTime();
            }

            if (hourInt < 0 || hourInt > 23)
                throw new InvalidPointInTime();

            return hourInt;
        }

        private int ParseOutMinute(string minuteString)
        {
            if (minuteString == "00" || 
                minuteString == "15" || 
                minuteString == "30" || 
                minuteString == "45")
                return minuteString.Parse<int>();

            throw new InvalidPointInTime();
        }
    }
}
