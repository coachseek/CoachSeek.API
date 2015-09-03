using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;

namespace CoachSeek.Application.UseCases
{
    public class PaypalReceivePaymentMessageUseCase : IPaypalReceivePaymentMessageUseCase
    {
        private IPaymentProcessingQueueClient PaymentProcessingQueueClient { get; set; }

        public PaypalReceivePaymentMessageUseCase(IPaymentProcessingQueueClient paymentProcessingQueueClient)
        {
            PaymentProcessingQueueClient = paymentProcessingQueueClient;
        }


        public void Receive(string formData)
        {
            var message = PaymentProcessingMessage.Create(Constants.PAYPAL, formData);
            PaymentProcessingQueueClient.Push(message);
        }
    }
}
