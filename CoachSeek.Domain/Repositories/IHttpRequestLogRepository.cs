using Coachseek.Logging.Contracts;

namespace CoachSeek.Domain.Repositories
{
    public interface IHttpRequestLogRepository
    {
        void Log(RequestLogMessage requestLogMessage);
    }
}
