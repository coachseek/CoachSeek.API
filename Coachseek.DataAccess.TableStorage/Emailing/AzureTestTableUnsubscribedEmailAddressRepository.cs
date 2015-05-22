namespace Coachseek.DataAccess.TableStorage.Emailing
{
    public class AzureTestTableUnsubscribedEmailAddressRepository : AzureTableUnsubscribedEmailAddressRepository
    {
        protected override string ConnectionStringKey { get { return "StorageConnectionString-Test"; } }
    }
}
