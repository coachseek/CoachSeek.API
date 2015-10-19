using System;
using System.Threading.Tasks;

namespace CoachSeek.Domain.Repositories
{
    public interface ILogRepository
    {
        Task LogErrorAsync(Exception error, string data = null);
        Task LogErrorAsync(string message, string data = null);
        Task LogInfoAsync(string message, string data = null);

        void LogError(Exception error, string data = null);
        void LogError(string message, string data = null);
        void LogInfo(string message, string data = null);
    }
}
