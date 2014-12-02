using System.Collections.Generic;
using CoachSeek.Domain.Contracts;

namespace CoachSeek.Domain.Entities
{
    public class DailySingleSessionListCalculator : ISingleSessionListCalculator
    {
        public IList<SingleSession> Calculate(SingleSession firstSession, SessionCount sessionCount)
        {
            var currentSession = firstSession;
            var sessions = new List<SingleSession> { firstSession };
            var count = 1; // Already got one session

            while (count < sessionCount.Count)
            {
                var currentStart = currentSession.Start;

                var nextStart = new PointInTime(currentStart);
                nextStart.AddDays(1);

                var nextSession = currentSession.Clone(nextStart.Date);
                sessions.Add(nextSession);

                count++;
                currentSession = nextSession;
            }

            return sessions;
        }
    }
}
