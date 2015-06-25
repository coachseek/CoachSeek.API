using System;
using CoachSeek.Domain.Repositories;

namespace Coachseek.Logging.Tests.Unit.Fakes
{
    public class StubLogRepository : ILogRepository
    {
        public bool WasLogErrorCalled;
        public bool WasLogInfoCalled;
        public string PassedInMessage;
        public string PassedInData;

        public void LogError(Exception error)
        {
            WasLogErrorCalled = true;
            PassedInMessage = error.Message;
        }

        public void LogError(Exception error, string data)
        {
            WasLogErrorCalled = true;
            PassedInMessage = error.Message;
            PassedInData = data;
        }

        public void LogError(string message)
        {
            WasLogErrorCalled = true;
            PassedInMessage = message;
        }

        public void LogInfo(string message, string data)
        {
            WasLogErrorCalled = true;
            PassedInMessage = message;
            PassedInData = data;
        }
    }
}
