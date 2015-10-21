using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coachseek.Infrastructure.Queueing.Contracts;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Coachseek.Infrastructure.Queueing.Azure
{
    public abstract class AzurePaymentProcessingQueueClient
    {
        protected abstract string QueueName { get; }

        protected AzureQueueClient AzureQueueClient { get; set; }


        protected AzurePaymentProcessingQueueClient()
        {
            AzureQueueClient = new AzureQueueClient();
        }
  

        private async Task<Queue> GetQueueAsync()
        {
            return await AzureQueueClient.GetQueueAsync(QueueName);
        }


        public async Task PushAsync(PaymentProcessingMessage message)
        {
            var azureMessage = ConvertToNewCloudQueueMessage(message);
            await AzureQueueClient.PushAsync(await GetQueueAsync(), azureMessage);
        }

        public async Task<IList<PaymentProcessingMessage>> PeekAsync()
        {
            var cloudMessages = await AzureQueueClient.PeekAsync(await GetQueueAsync());
            return ConvertToPaymentProcessingMessages(cloudMessages);
        }

        public async Task PopAsync(PaymentProcessingMessage message)
        {
            var azureMessage = ConvertToExistingCloudQueueMessage(message);
            await AzureQueueClient.PopAsync(await GetQueueAsync(), azureMessage);
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
