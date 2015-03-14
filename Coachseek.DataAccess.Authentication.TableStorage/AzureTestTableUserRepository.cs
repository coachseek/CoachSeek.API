namespace Coachseek.DataAccess.Authentication.TableStorage
{
    public class AzureTestTableUserRepository : AzureTableUserRepository
    {
        protected override string ConnectionStringKey { get { return "StorageConnectionString-Test"; } } 
    }
}
