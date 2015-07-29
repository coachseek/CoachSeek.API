namespace Coachseek.DataAccess.TableStorage.Logging
{
    public class AzureTestTableHttpRequestLogRepository : AzureTableHttpRequestLogRepository
    {
        protected override string ConnectionStringKey { get { return "StorageConnectionString-Test"; } }
    }
}
