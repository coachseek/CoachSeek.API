using System;

namespace CoachSeek.Domain.Repositories
{
    public interface ILogRepository
    {
        void LogError(Exception error);
        void LogError(string message);
    }
}
