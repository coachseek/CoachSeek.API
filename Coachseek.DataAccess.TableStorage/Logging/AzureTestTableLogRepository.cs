namespace Coachseek.DataAccess.TableStorage.Logging
{
    public class AzureTestTableLogRepository : AzureTableLogRepository
    {
        protected override string ConnectionStringKey { get { return "StorageConnectionString-Test"; } }

        public AzureTestTableLogRepository(string application) 
            : base(application)
        { }
    }
}
