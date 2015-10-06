
using System;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class DateOfBirth : Date
    {
        public DateOfBirth(string dateString) 
            : base(dateString)
        {
            if (_date < new DateTime(1900, 1, 1))
                throw new DateOfBirthInvalid(ToString());
            if (_date > DateTime.Today)
                throw new DateOfBirthInvalid(ToString());
        }

        public DateOfBirth(DateTime date) : base(date)
        {
        }
    }
}
