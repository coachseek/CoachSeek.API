﻿using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class Duration
    {
        private const int MINUTES_IN_ONE_DAY = 1440;
        private const int MINUTES_IN_INCREMENT = 15;

        public int? Minutes { get; private set; }

        public Duration(int? minutes)
        {
            Minutes = minutes;

            Validate();
        }

        private void Validate()
        {
            if (Minutes == null)
                return;
            if (Minutes.Value <= 0)
                throw new InvalidDuration();
            if (Minutes.Value > MINUTES_IN_ONE_DAY)
                throw new InvalidDuration();
            if (Minutes.Value % MINUTES_IN_INCREMENT > 0)
                throw new InvalidDuration();
        }
    }
}
