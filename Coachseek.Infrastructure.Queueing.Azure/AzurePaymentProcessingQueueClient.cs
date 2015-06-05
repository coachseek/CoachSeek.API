﻿using System.Collections.Generic;
using System.Linq;
using Coachseek.Infrastructure.Queueing.Contracts;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Coachseek.Infrastructure.Queueing.Azure
{
    public class AzurePaymentProcessingQueueClient : IPaymentProcessingQueueClient
    {
        private const string QUEUE_NAME = "payments";

        private AzureQueueClient AzureQueueClient { get; set; }


        public AzurePaymentProcessingQueueClient()
        {
            AzureQueueClient = new AzureQueueClient();
        }
  

        private Queue Queue
        {
            get { return AzureQueueClient.GetQueue(QUEUE_NAME); }
        }


        public void Push(PaymentProcessingMessage message)
        {
            var azureMessage = ConvertToCloudQueueMessage(message);
            AzureQueueClient.Push(Queue, azureMessage);
        }

        public IList<PaymentProcessingMessage> Peek()
        {
            var cloudMessages = AzureQueueClient.Peek(Queue);
            return ConvertToPaymentProcessingMessages(cloudMessages);
        }

        public void Pop(PaymentProcessingMessage message)
        {
            var azureMessage = ConvertToCloudQueueMessage(message);
            AzureQueueClient.Pop(Queue, azureMessage);
        }


        private CloudQueueMessage ConvertToCloudQueueMessage(PaymentProcessingMessage message)
        {
            return new CloudQueueMessage(message.ToString());
        }

        private IList<PaymentProcessingMessage> ConvertToPaymentProcessingMessages(IEnumerable<CloudQueueMessage> messages)
        {
            return messages.Select(ConvertToPaymentProcessingMessage).ToList();
        }

        private PaymentProcessingMessage ConvertToPaymentProcessingMessage(CloudQueueMessage message)
        {
            return new PaymentProcessingMessage(message.AsString);
        }
    }
}
