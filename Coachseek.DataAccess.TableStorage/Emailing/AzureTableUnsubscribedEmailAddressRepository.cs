using CoachSeek.Domain.Repositories;
using Microsoft.WindowsAzure.Storage.Table;

namespace Coachseek.DataAccess.TableStorage.Emailing
{
    public class AzureTableUnsubscribedEmailAddressRepository : AzureTableRepositoryBase, IUnsubscribedEmailAddressRepository
    {
        protected override string TableName { get { return "unsubscribed-email-addresses"; } }


        public void Save(string emailAddress)
        {
            var address = new EmailAddressEntity(emailAddress)
            {
                EmailAddress = emailAddress,
            };

            Table.Execute(TableOperation.Insert(address));
        }


        //public User Get(Guid id)
        //{
        //    var query = new TableQuery<Authentication.UserEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Constants.USER));

        //    foreach (var user in UsersTable.ExecuteQuery(query))
        //    {
        //        if (user.Id == id)
        //            return new User(user.Id, user.BusinessId, user.BusinessName, user.Email, user.FirstName, user.LastName, user.RowKey, user.PasswordHash);
        //    }

        //    return null;
        //}

        //public User GetByUsername(string username)
        //{
        //    var retrieveOperation = TableOperation.Retrieve<Authentication.UserEntity>(Constants.USER, username);

        //    var retrievedResult = UsersTable.Execute(retrieveOperation);
        //    if (retrievedResult.Result == null)
        //        return null;

        //    var user = (Authentication.UserEntity) retrievedResult.Result;

        //    return new User(user.Id, user.BusinessId, user.BusinessName, user.Email, user.FirstName, user.LastName, user.RowKey, user.PasswordHash);
        //}
    }
}
