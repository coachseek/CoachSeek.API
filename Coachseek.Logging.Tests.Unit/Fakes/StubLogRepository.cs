using System;
using System.Threading.Tasks;
using CoachSeek.Domain.Repositories;

namespace Coachseek.Logging.Tests.Unit.Fakes
{
    public class StubLogRepository : ILogRepository
    {
        public bool WasLogErrorCalled;
        public bool WasLogInfoCalled;
        public string PassedInMessage;
        public string PassedInData;


        public async Task LogErrorAsync(Exception error, string data = null)
        {
            await Task.Delay(100);
            LogError(error, data);
        }

        public async Task LogErrorAsync(string message, string data = null)
        {
            await Task.Delay(100);
            LogError(message, data);
        }

        public async Task LogInfoAsync(string message, string data = null)
        {
            await Task.Delay(100);
            LogInfo(message, data);
        }


        public void LogError(Exception error, string data = null)
        {
            WasLogErrorCalled = true;
            PassedInMessage = error.Message;
            PassedInData = data;
        }

        public void LogError(string message, string data = null)
        {
            WasLogErrorCalled = true;
            PassedInMessage = message;
        }

        public void LogInfo(string message, string data = null)
        {
            WasLogInfoCalled = true;
            PassedInMessage = message;
            PassedInData = data;
        }
    }
}
