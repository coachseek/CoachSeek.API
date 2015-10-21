using System.Threading.Tasks;
using CoachSeek.Application.Contracts.UseCases.Payments;
using CoachSeek.Common;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;

namespace CoachSeek.Application.UseCases.Payments
{
    public class PaypalReceiveOnlinePaymentMessageUseCase : IPaypalReceiveOnlinePaymentMessageUseCase
    {
        private IOnlinePaymentProcessingQueueClient OnlinePaymentProcessingQueueClient { get; set; }

        public PaypalReceiveOnlinePaymentMessageUseCase(IOnlinePaymentProcessingQueueClient onlinePaymentProcessingQueueClient)
        {
            OnlinePaymentProcessingQueueClient = onlinePaymentProcessingQueueClient;
        }


        public async Task ReceiveAsync(string formData)
        {
            var message = PaymentProcessingMessage.Create(Constants.PAYPAL, formData);
            await OnlinePaymentProcessingQueueClient.PushAsync(message);
        }
    }
}
