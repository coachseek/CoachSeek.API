using System.Collections.Generic;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Domain.Contracts
{
    public interface ISingleSessionListCalculator
    {
        IList<SingleSession> Calculate(SingleSession firstSession, SessionCount sessionCount);
    }
}
