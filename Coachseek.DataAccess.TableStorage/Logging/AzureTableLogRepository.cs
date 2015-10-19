using System;
using System.Threading.Tasks;
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


        public async Task LogErrorAsync(Exception error, string data = null)
        {
            await LogErrorAsync(error.Message, data);
        }

        public async Task LogErrorAsync(string message, string data = null)
        {
            await LogAsync(message, data, LogLevel.Error);
        }

        public async Task LogInfoAsync(string message, string data = null)
        {
            await LogAsync(message, data, LogLevel.Info);
        }


        public void LogError(Exception error, string data = null)
        {
            LogError(error.Message, data);
        }

        public void LogError(string message, string data = null)
        {
            Log(message, data, LogLevel.Error);
        }

        public void LogInfo(string message, string data = null)
        {
            Log(message, data, LogLevel.Info);
        }


        private async Task LogAsync(string message, string data, LogLevel level)
        {
            var logEntity = CreateLogEntity(message, data, level);
            await Table.ExecuteAsync(TableOperation.Insert(logEntity));
        }

        private void Log(string message, string data, LogLevel level)
        {
            var logEntity = CreateLogEntity(message, data, level);
            Table.Execute(TableOperation.Insert(logEntity));
        }

        private LogEntity CreateLogEntity(string message, string data, LogLevel level)
        {
            var logMessage = new LogMessage(Application, level, message, DateTime.UtcNow, data);
            return new LogEntity(logMessage);
        }
    }
}
