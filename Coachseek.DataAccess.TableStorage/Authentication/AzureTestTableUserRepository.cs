namespace Coachseek.DataAccess.TableStorage.Authentication
{
    public class AzureTestTableUserRepository : AzureTableUserRepository
    {
        protected override string ConnectionStringKey { get { return "StorageConnectionString-Test"; } } 
    }
}
