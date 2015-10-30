using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coachseek.Infrastructure.Queueing.Contracts;
using Coachseek.Infrastructure.Queueing.Contracts.Import;
using Coachseek.Integration.Contracts.DataImport;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Coachseek.Infrastructure.Queueing.Azure
{
    public class AzureDataImportQueueClient : IDataImportQueueClient
    {
        private const string QUEUE_NAME = "imports";

        private AzureQueueClient AzureQueueClient { get; set; }


        public AzureDataImportQueueClient()
        {
            AzureQueueClient = new AzureQueueClient();
        }
  

        private Queue Queue
        {
            get { return AzureQueueClient.GetQueue(QUEUE_NAME); }
        }


        public async Task PushAsync(DataImportMessage message)
        {
            var azureMessage = ConvertToNewCloudQueueMessage(message);
            try
            {
                await AzureQueueClient.PushAsync(Queue, azureMessage);
            }
            catch (Exception ex)
            {
                throw new DataImportException(ex.Message, ex);
            }
        }

        public async Task<IList<DataImportMessage>> PeekAsync()
        {
            var cloudMessages = await AzureQueueClient.PeekAsync(Queue);
            return ConvertToDataImportMessages(cloudMessages);
        }

        public async Task PopAsync(DataImportMessage message)
        {
            var azureMessage = ConvertToExistingCloudQueueMessage(message);
            await AzureQueueClient.PopAsync(Queue, azureMessage);
        }


        private CloudQueueMessage ConvertToNewCloudQueueMessage(DataImportMessage message)
        {
            return new CloudQueueMessage(message.ToString());
        }

        private CloudQueueMessage ConvertToExistingCloudQueueMessage(DataImportMessage message)
        {
            var parts = message.Id.Split('|');
            var cloudMessage = new CloudQueueMessage(parts[0], parts[1]);
            cloudMessage.SetMessageContent(message.Contents);
            return cloudMessage;
        }

        private IList<DataImportMessage> ConvertToDataImportMessages(IEnumerable<CloudQueueMessage> messages)
        {
            return messages.Select(ConvertToDataImportMessage).ToList();
        }

        private DataImportMessage ConvertToDataImportMessage(CloudQueueMessage message)
        {
            var messageId = string.Format("{0}|{1}", message.Id, message.PopReceipt);
            return new DataImportMessage(messageId, message.AsString);
        }
    }
}
