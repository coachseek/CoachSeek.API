using System;
using Coachseek.Logging.Contracts;
using Microsoft.WindowsAzure.Storage.Table;

namespace Coachseek.DataAccess.TableStorage.Logging
{
    public class LogEntity : TableEntity
    {
        public LogEntity(LogMessage logMessage)
        {
            PartitionKey = logMessage.Date;
            RowKey = logMessage.Time;

            Id = logMessage.Id;
            Application = logMessage.Application;
            LogLevel = logMessage.LogLevel.ToString();
            Message = logMessage.Message;
            DateTime = logMessage.DateTime;
            Data = logMessage.Data;
        }

        public Guid Id { get; set; }
        public string Application { get; set; }
        public string LogLevel { get; set; }
        public string Message { get; set; }
        public DateTime DateTime { get; set; }
        public string Data { get; set; }
    }
}
