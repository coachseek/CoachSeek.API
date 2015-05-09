using System;
using Coachseek.DataAccess.Authentication.TableStorage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Coachseek.DataAccess.TableStorage.Emailing
{
    public class EmailEntity : TableEntity
    {
        public EmailEntity(string username)
        {
            PartitionKey = Constants.EMAIL;
            RowKey = username;
        }

        public EmailEntity() { }


        public Guid Id { get; set; }

        public string BusinessName { get; set; } // Debug

        public string Type { get; set; }

        public string Sender { get; set; }
        public string Recipient { get; set; }

        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
