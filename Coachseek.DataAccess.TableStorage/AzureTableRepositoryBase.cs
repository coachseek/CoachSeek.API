using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Coachseek.DataAccess.TableStorage
{
    public abstract class AzureTableRepositoryBase
    {
        protected abstract string TableName { get; }

        protected CloudTableClient TableClient { get; set; }

        protected virtual string ConnectionStringKey { get { return "StorageConnectionString"; } }

        protected CloudStorageAccount StorageAccount
        {
            get
            {
                var connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringKey].ConnectionString;
                return CloudStorageAccount.Parse(connectionString);
            }
        }

        protected CloudTable Table
        {
            get
            {
                TableClient = StorageAccount.CreateCloudTableClient();

                var table = TableClient.GetTableReference(TableName);
                table.CreateIfNotExists();

                return table;
            }
        }
    }
}
