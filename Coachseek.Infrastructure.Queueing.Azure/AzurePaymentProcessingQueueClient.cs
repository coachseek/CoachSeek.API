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


        public void PushPaymentProcessingMessageOntoQueue(PaymentProcessingMessage message)
        {
            var azureMessage = ConvertToCloudQueueMessages(message);
            AzureQueueClient.PushMessageOntoQueue(Queue, azureMessage);
        }


        private CloudQueueMessage ConvertToCloudQueueMessages(PaymentProcessingMessage message)
        {
            return new CloudQueueMessage(message.ToString());
        }
    }
}
