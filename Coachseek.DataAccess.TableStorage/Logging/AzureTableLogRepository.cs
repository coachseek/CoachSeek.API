using System;
using CoachSeek.Domain.Repositories;
using Coachseek.Logging.Contracts;
using Microsoft.WindowsAzure.Storage.Table;

namespace Coachseek.DataAccess.TableStorage.Logging
{
    public class AzureTableLogRepository : AzureTableRepositoryBase, ILogRepository
    {
        protected override string TableName { get { return "log"; } }

        private string Application { set; get; }


        public AzureTableLogRepository(string application)
        {
            Application = application;
        }

        public void LogError(Exception error)
        {
            LogError(error.Message);
        }

        public void LogError(Exception error, string data)
        {
            LogError(error.Message, data);
        }

        public void LogError(string  message)
        {
            var logMessage = new LogMessage(Application, LogLevel.Error, message, DateTime.Now);
            var logEntity = new LogEntity(logMessage);
            Table.Execute(TableOperation.Insert(logEntity));
        }

        public void LogError(string message, string data)
        {
            Log(message, data, LogLevel.Error);
        }

        public void LogInfo(string message, string data)
        {
            Log(message, data, LogLevel.Info);
        }


        private void Log(string message, string data, LogLevel level)
        {
            var logMessage = new LogMessage(Application, level, message, DateTime.Now, data);
            var logEntity = new LogEntity(logMessage);
            Table.Execute(TableOperation.Insert(logEntity));
        }
    }
}
