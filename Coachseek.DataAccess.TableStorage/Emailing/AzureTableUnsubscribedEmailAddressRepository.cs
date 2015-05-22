using CoachSeek.Domain.Repositories;
using Microsoft.WindowsAzure.Storage.Table;

namespace Coachseek.DataAccess.TableStorage.Emailing
{
    public class AzureTableUnsubscribedEmailAddressRepository : AzureTableRepositoryBase, IUnsubscribedEmailAddressRepository
    {
        protected override string TableName { get { return "unsubscribed"; } }


        public void Save(string emailAddress)
        {
            if (Get(emailAddress)) 
                return;

            var address = new EmailAddressEntity(emailAddress) { EmailAddress = emailAddress };
            Table.Execute(TableOperation.Insert(address));
        }


        public bool Get(string emailAddress)
        {
            var parts = emailAddress.Split('@');
            var retrieveOperation = TableOperation.Retrieve<EmailAddressEntity>(parts[1], parts[0]);

            var retrievedResult = Table.Execute(retrieveOperation);
            return retrievedResult.Result != null;
        }
    }
}
