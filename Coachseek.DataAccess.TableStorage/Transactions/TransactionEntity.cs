using Microsoft.WindowsAzure.Storage.Table;

namespace Coachseek.DataAccess.TableStorage.Transactions
{
    public class TransactionEntity : TableEntity
    {
        public TransactionEntity(string emailAddress)
        {
            var parts = emailAddress.Split('@');

            PartitionKey = parts[1];    // domain eg. coachseek.com
            RowKey = parts[0];          // local part eg. olaf
        }

        public TransactionEntity() { }

        public string EmailAddress { get; set; } // Debug
    }
}
