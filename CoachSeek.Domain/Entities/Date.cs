﻿using System;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class Date
    {
        private DateTime _date;

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

        public string ToData()
        {
            return string.Format("{0:yyyy-MM-dd}", _date);
        }
    }
}
