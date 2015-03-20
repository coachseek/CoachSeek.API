using System.Collections.Generic;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Domain.Contracts
{
    public interface ISingleSessionListCalculator
    {
        IList<SessionInCourse> Calculate(SessionInCourse firstSession, SessionCount sessionCount);
        IList<SessionInCourse> Calculate(SessionInCourse firstSession, int sessionCount);
    }
}
