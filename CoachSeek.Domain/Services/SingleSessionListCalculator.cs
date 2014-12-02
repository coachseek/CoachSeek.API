﻿using System.Collections.Generic;
using CoachSeek.Domain.Contracts;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Domain.Services
{
    public abstract class SingleSessionListCalculator : ISingleSessionListCalculator
    {
        protected abstract int OffsetNumberOfDays { get; }

        public IList<SingleSession> Calculate(SingleSession firstSession, SessionCount sessionCount)
        {
            return Calculate(firstSession, sessionCount.Count);
        }

        public IList<SingleSession> Calculate(SingleSession firstSession, int sessionCount)
        {
            var currentSession = firstSession;
            var sessions = new List<SingleSession> { firstSession };
            var count = 1; // Already got one session

            while (count < sessionCount)
            {
                var currentStart = currentSession.Start;

                var nextStart = new PointInTime(currentStart);
                nextStart.AddDays(OffsetNumberOfDays);

                var nextSession = currentSession.Clone(nextStart.Date);
                sessions.Add(nextSession);

                count++;
                currentSession = nextSession;
            }

            return sessions;
        }
    }
}
