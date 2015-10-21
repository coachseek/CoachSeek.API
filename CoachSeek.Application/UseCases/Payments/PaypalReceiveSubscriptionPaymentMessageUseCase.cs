using System.Threading.Tasks;
using CoachSeek.Application.Contracts.UseCases.Payments;
using CoachSeek.Common;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;

namespace CoachSeek.Application.UseCases.Payments
{
    public class PaypalReceiveSubscriptionPaymentMessageUseCase : IPaypalReceiveSubscriptionPaymentMessageUseCase
    {
        private ISubscriptionPaymentProcessingQueueClient SubscriptionPaymentProcessingQueueClient { get; set; }

        public PaypalReceiveSubscriptionPaymentMessageUseCase(ISubscriptionPaymentProcessingQueueClient subscriptionPaymentProcessingQueueClient)
        {
            SubscriptionPaymentProcessingQueueClient = subscriptionPaymentProcessingQueueClient;
        }


        public async Task ReceiveAsync(string formData)
        {
            var message = PaymentProcessingMessage.Create(Constants.PAYPAL, formData);
            await SubscriptionPaymentProcessingQueueClient.PushAsync(message);
        }
    }
}
