using Coachseek.Infrastructure.Queueing.Contracts.Payment;

namespace Coachseek.Infrastructure.Queueing.Azure
{
    public class AzureOnlinePaymentProcessingQueueClient : AzurePaymentProcessingQueueClient, IOnlinePaymentProcessingQueueClient
    {
        protected override string QueueName { get { return "payments"; } }
    }
}
