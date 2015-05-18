using System.Configuration;
using Coachseek.Integration.Contracts.Interfaces;
using Coachseek.Integration.Contracts.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Coachseek.DataAccess.TableStorage.Emailing
{
    public class AzureTableEmailRepository : IEmailer
    {
        private const string TABLE_NAME = "emails";

        private CloudTableClient TableClient { get; set; }

        protected virtual string ConnectionStringKey { get { return "StorageConnectionString"; } } 

        private CloudStorageAccount StorageAccount
        {
            get
            {
                //var connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringKey].ConnectionString;
                //return CloudStorageAccount.Parse(connectionString);
                return null;
            }
        }

        private CloudTable EmailsTable
        {
            get
            {
                //TableClient = StorageAccount.CreateCloudTableClient();

                //var emailsTable = TableClient.GetTableReference(TABLE_NAME);
                //emailsTable.CreateIfNotExists();

                //return emailsTable;
                return null;
            }
        }


        public bool Send(Email email)
        {
            //var emailEntity = new EmailEntity(email)
            //{
            //    Sender = email.Sender,
            //    Recipient = email.Recipient,
            //    Subject = email.Subject,
            //    Body = email.Body
            //};

            //EmailsTable.Execute(TableOperation.Insert(emailEntity));
            return false;
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
