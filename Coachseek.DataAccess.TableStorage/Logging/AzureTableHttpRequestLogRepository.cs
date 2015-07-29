using CoachSeek.Domain.Repositories;
using Coachseek.Logging.Contracts;
using Microsoft.WindowsAzure.Storage.Table;

namespace Coachseek.DataAccess.TableStorage.Logging
{
    public class AzureTableHttpRequestLogRepository : AzureTableRepositoryBase, IHttpRequestLogRepository
    {
        protected override string TableName { get { return "requestlog"; } }


        public void Log(RequestLogMessage requestLogMessage)
        {
            var requestLog = new RequestLogEntity(requestLogMessage);

            Table.Execute(TableOperation.Insert(requestLog));
        }
    }
}
