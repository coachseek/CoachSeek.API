using System;
using CoachSeek.Domain.Repositories;

namespace Coachseek.Logging.Tests.Unit.Fakes
{
    public class StubLogRepository : ILogRepository
    {
        public bool WasLogErrorCalled;
        public string PassedInMessage;

        public void LogError(Exception error)
        {
            WasLogErrorCalled = true;
            PassedInMessage = error.Message;
        }

        public void LogError(string message)
        {
            WasLogErrorCalled = true;
            PassedInMessage = message;
        }
    }
}
