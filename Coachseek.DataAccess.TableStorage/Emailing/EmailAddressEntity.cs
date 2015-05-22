using Microsoft.WindowsAzure.Storage.Table;

namespace Coachseek.DataAccess.TableStorage.Emailing
{
    public class EmailAddressEntity : TableEntity
    {
        public EmailAddressEntity(string emailAddress)
        {
            var parts = emailAddress.Split('@');

            PartitionKey = parts[1];    // domain eg. coachseek.com
            RowKey = parts[0];          // local part eg. olaf
        }

        public EmailAddressEntity() { }

        public string EmailAddress { get; set; } // Debug
    }
}
