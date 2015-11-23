﻿using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Exceptions;
using System;

namespace CoachSeek.Domain.Entities
{
    public class TimeOfDay
    {
        public int Hour { get; private set; }
        public int Minute { get; private set; }


        public TimeOfDay(string time)
        {
            if (time == null)
                throw new TimeInvalid(time);

            try
            {
                var components = SplitIntoHourAndMinute(time);
                Hour = ParseOutHour(components[0]);
                Minute = ParseOutMinute(components[1]);
            }
            catch (FormatException)
            {
                throw new TimeInvalid(time);
            }
        }

        public TimeOfDay(DateTime time)
        {
            Hour = time.Hour;
            Minute = time.Minute;
        }


        public bool IsAfter(TimeOfDay earlier)
        {
            if (earlier.Hour > Hour)
                return false;
            if (earlier.Hour == Hour && earlier.Minute >= Minute)
                    return false;                

            return true;
        }

        public override string ToString()
        {
            return string.Format("{0:D2}:{1:D2}", Hour, Minute);
        }


        private static string[] SplitIntoHourAndMinute(string time)
        {
            var components = time.Trim().Split(':');
            if (components.GetLength(0) != 2)
                throw new FormatException();
            return components;
        }

        private int ParseOutHour(string hourString)
        {
            if (hourString.Length < 1 || hourString.Length > 2)
                throw new FormatException();

            var hourInt = hourString.ParseOrThrow<int>();

            if (hourInt < 0 || hourInt > 23)
                throw new FormatException();

            return hourInt;
        }

        private int ParseOutMinute(string minuteString)
        {
            if (minuteString == "00" || 
                minuteString == "15" || 
                minuteString == "30" || 
                minuteString == "45")
                return minuteString.Parse<int>();

            throw new FormatException();
        }
    }
}
