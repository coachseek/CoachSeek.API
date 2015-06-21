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

        public void LogError(string  message)
        {
            var logMessage = new LogMessage(Application, LogLevel.Error, message, DateTime.Now);
            var logEntity = new LogEntity(logMessage);
            Table.Execute(TableOperation.Insert(logEntity));
        }
    }
}
