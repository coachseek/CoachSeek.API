using System;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.UseCases;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;
using Coachseek.Integration.Contracts.Payments.Interfaces;

namespace CoachSeek.Application.UseCases
{
    public class ProcessPaymentsUseCase : IProcessPaymentsUseCase
    {
        public IPaymentProcessingQueueClient PaymentProcessingQueueClient { get; private set; }
        public IPaymentMessageProcessor PaymentMessageProcessor { get; private set; }


        public ProcessPaymentsUseCase(IPaymentProcessingQueueClient paymentProcessingQueueClient,
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
