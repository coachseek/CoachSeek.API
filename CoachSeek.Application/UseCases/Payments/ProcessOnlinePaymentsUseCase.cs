using System;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.UseCases.Payments;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;
using Coachseek.Integration.Contracts.Payments.Interfaces;

namespace CoachSeek.Application.UseCases.Payments
{
    public class ProcessOnlinePaymentsUseCase : IProcessOnlinePaymentsUseCase
    {
        public IOnlinePaymentProcessingQueueClient PaymentProcessingQueueClient { get; private set; }
        public IPaymentMessageProcessor PaymentMessageProcessor { get; private set; }


        public ProcessOnlinePaymentsUseCase(IOnlinePaymentProcessingQueueClient paymentProcessingQueueClient,
                                            IPaymentMessageProcessor paymentMessageProcessor)
        {
            PaymentProcessingQueueClient = paymentProcessingQueueClient;
            PaymentMessageProcessor = paymentMessageProcessor;
        }


        public async Task ProcessAsync()
        {
            var messages = await PaymentProcessingQueueClient.PeekAsync();

            foreach (var message in messages)
            {
                try
                {
                    await PaymentMessageProcessor.ProcessMessageAsync(message);
                    await PaymentProcessingQueueClient.PopAsync(message);
                }
                catch (Exception)
                {
                    // Log error
                }
            }
        }
    }
}
