using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        private async Task<Queue> GetQueueAsync()
        {
            return await AzureQueueClient.GetQueueAsync(QUEUE_NAME);
        }


        public async Task PushAsync(PaymentProcessingMessage message)
        {
            var azureMessage = ConvertToNewCloudQueueMessage(message);
            await AzureQueueClient.PushAsync(Queue, azureMessage);
        }

        public void Push(PaymentProcessingMessage message)
        {
            var azureMessage = ConvertToNewCloudQueueMessage(message);
            AzureQueueClient.Push(Queue, azureMessage);
        }

        public async Task<IList<PaymentProcessingMessage>> PeekAsync()
        {
            var queue = await GetQueueAsync();
            var cloudMessages = await AzureQueueClient.PeekAsync(queue);
            return ConvertToPaymentProcessingMessages(cloudMessages);
        }

        public IList<PaymentProcessingMessage> Peek()
        {
            var cloudMessages = AzureQueueClient.Peek(Queue);
            return ConvertToPaymentProcessingMessages(cloudMessages);
        }

        public async Task PopAsync(PaymentProcessingMessage message)
        {
            var azureMessage = ConvertToExistingCloudQueueMessage(message);
            await AzureQueueClient.PopAsync(Queue, azureMessage);
        }

        public void Pop(PaymentProcessingMessage message)
        {
            var azureMessage = ConvertToExistingCloudQueueMessage(message);
            AzureQueueClient.Pop(Queue, azureMessage);
        }


        private CloudQueueMessage ConvertToNewCloudQueueMessage(PaymentProcessingMessage message)
        {
            return new CloudQueueMessage(message.ToString());
        }

        private CloudQueueMessage ConvertToExistingCloudQueueMessage(PaymentProcessingMessage message)
        {
            var parts = message.Id.Split('|');
            var cloudMessage = new CloudQueueMessage(parts[0], parts[1]);
            cloudMessage.SetMessageContent(message.Contents);
            return cloudMessage;
        }

        private IList<PaymentProcessingMessage> ConvertToPaymentProcessingMessages(IEnumerable<CloudQueueMessage> messages)
        {
            return messages.Select(ConvertToPaymentProcessingMessage).ToList();
        }

        private PaymentProcessingMessage ConvertToPaymentProcessingMessage(CloudQueueMessage message)
        {
            var messageId = string.Format("{0}|{1}", message.Id, message.PopReceipt);
            return new PaymentProcessingMessage(messageId, message.AsString);
        }
    }
}
