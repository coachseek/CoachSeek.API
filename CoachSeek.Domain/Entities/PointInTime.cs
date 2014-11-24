using System;

namespace CoachSeek.Domain.Entities
{
    public class PointInTime
    {
        public DateTime DateTime { get; private set; }

        public PointInTime(Date date, TimeOfDay time)
        {
            DateTime = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, 0);
        }

        public PointInTime(PointInTime input)
        {
            DateTime = new DateTime(input.DateTime.Ticks);
        }


        public void AddMinutes(int minutes)
        {
            DateTime = DateTime.AddMinutes(minutes);
        }

        public bool IsAfter(PointInTime other)
        {
            return DateTime.CompareTo(other.DateTime) > 0;
        }
    }
}
