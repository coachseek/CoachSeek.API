using System;
using System.Collections.Generic;
using CoachSeek.Domain.Contracts;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Domain.Services
{
    public class MonthlySingleSessionListCalculator : ISingleSessionListCalculator
    {
        public IList<SingleSession> Calculate(SingleSession firstSession, SessionCount sessionCount)
        {
            throw new NotImplementedException();
        }
        public IList<SingleSession> Calculate(SingleSession firstSession, int sessionCount)
        {
            throw new NotImplementedException();
        }
    }
}
