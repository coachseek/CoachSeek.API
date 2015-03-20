using System;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class Date
    {
        internal DateTime _date;

        public int Year { get { return _date.Year; } }
        public int Month { get { return _date.Month; } }
        public int Day { get { return _date.Day; } }


        public Date(string dateString)
        {
            DateTime date;
            var succeeded = DateTime.TryParse(dateString, out date);
            if (!succeeded)
                throw new InvalidDate();
            _date = date;
        }

        public Date(string dateString, int dayOffset)
            : this(dateString)
        {
            _date = _date.AddDays(dayOffset);
        }

        public Date(DateTime date)
        {
            _date = date;
        }

        public string ToData()
        {
            return string.Format("{0:yyyy-MM-dd}", _date);
        }

        public DateTime ToDateTime()
        {
            return _date;
        }

        public bool IsAfter(Date otherDate)
        {
            return _date.Ticks > otherDate._date.Ticks;
        }

        public bool IsOnOrAfter(Date otherDate)
        {
            return _date.Ticks >= otherDate._date.Ticks;
        }

        public bool IsOnOrBefore(Date otherDate)
        {
            return _date.Ticks <= otherDate._date.Ticks;
        }

        public bool IsBefore(Date otherDate)
        {
            return _date.Ticks < otherDate._date.Ticks;
        }

        public int CalculateDayOffsetTo(Date otherDate)
        {
            var difference = _date.Subtract(otherDate.ToDateTime());
            return difference.Days;
        }
    }
}
