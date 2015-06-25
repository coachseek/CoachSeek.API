using System;

namespace Coachseek.Logging.Contracts
{
    public class LogMessage
    {
        public Guid Id { get; private set; }
        public string Application { get; private set; }
        public LogLevel LogLevel { get; private set; }
        public string Message { get; private set; }
        public DateTime DateTime { get; private set; }
        public string Date { get { return DateTime.ToString("yyyy-MM-dd"); ; } }
        public string Time { get { return DateTime.ToString("hh:mm"); ; } }
        public string Data { get; private set; }


        public LogMessage(string application, LogLevel logLevel, string message, DateTime dateTime, string data = null)
        {
            Id = Guid.NewGuid();
            Application = application;
            LogLevel = logLevel;
            Message = message;
            DateTime = dateTime;
            Data = data;
        }
    }
}
