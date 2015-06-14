using System;
using CoachSeek.Application.Contracts.UseCases;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;
using Coachseek.Integration.Contracts.Interfaces;

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


        public void Process()
        {
            var messages = PaymentProcessingQueueClient.Peek();

            foreach (var message in messages)
            {
                try
                {
                    PaymentMessageProcessor.ProcessMessage(message);
                    PaymentProcessingQueueClient.Pop(message);
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
