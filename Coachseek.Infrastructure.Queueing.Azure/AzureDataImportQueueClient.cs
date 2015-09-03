﻿using System.Collections.Generic;
using System.Linq;
using Coachseek.Infrastructure.Queueing.Contracts;
using Coachseek.Infrastructure.Queueing.Contracts.Import;
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


        public void Push(DataImportMessage message)
        {
            var azureMessage = ConvertToNewCloudQueueMessage(message);
            AzureQueueClient.Push(Queue, azureMessage);
        }

        public IList<DataImportMessage> Peek()
        {
            var cloudMessages = AzureQueueClient.Peek(Queue);
            return ConvertToDataImportMessages(cloudMessages);
        }

        public void Pop(DataImportMessage message)
        {
            var azureMessage = ConvertToExistingCloudQueueMessage(message);
            AzureQueueClient.Pop(Queue, azureMessage);
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
