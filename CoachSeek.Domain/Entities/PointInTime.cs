using System;

namespace CoachSeek.Domain.Entities
{
    public class PointInTime
    {
        public DateTime DateTime { get; private set; }
        public Date Date { get { return new Date(DateTime); } }
        public TimeOfDay Time { get { return new TimeOfDay(DateTime); } }


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

        public void AddDays(int days)
        {
            DateTime = DateTime.AddDays(days);
        }

        public bool IsAfter(PointInTime other)
        {
            return DateTime.CompareTo(other.DateTime) > 0;
        }

        public bool IsSameAs(PointInTime other)
        {
            return DateTime.CompareTo(other.DateTime) == 0;
        }
    }
}
