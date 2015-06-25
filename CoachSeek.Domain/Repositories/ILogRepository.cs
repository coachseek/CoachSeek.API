using System;

namespace CoachSeek.Domain.Repositories
{
    public interface ILogRepository
    {
        void LogError(Exception error);
        void LogError(Exception error, string data);
        void LogError(string message);

        void LogInfo(string message, string data);
    }
}
