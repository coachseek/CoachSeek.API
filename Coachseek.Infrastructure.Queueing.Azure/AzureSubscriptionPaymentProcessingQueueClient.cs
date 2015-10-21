using Coachseek.Infrastructure.Queueing.Contracts.Payment;

namespace Coachseek.Infrastructure.Queueing.Azure
{
    public class AzureSubscriptionPaymentProcessingQueueClient : AzurePaymentProcessingQueueClient, ISubscriptionPaymentProcessingQueueClient
    {
        protected override string QueueName { get { return "subscriptions"; } }
    }
}
