using System;
using System.Threading.Tasks;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;
using Coachseek.Integration.Contracts.Payments.Interfaces;

namespace Coachseek.Integration.Tests.Unit.Payments.Fakes
{
    public class StubPaymentMessageProcessor : IPaymentMessageProcessor
    {
        public bool WasProcessMessageCalled;
        public int ProcessMessageCallCount = 0;
        public PaymentProcessingMessage PassedInMessage;

        public async Task ProcessMessageAsync(PaymentProcessingMessage message)
        {
            await Task.Delay(100);
            WasProcessMessageCalled = true;
            ProcessMessageCallCount++;
            PassedInMessage = message;

            if (message.PaymentProvider == "Error")
                throw new Exception("error");
        }

        public void ProcessMessage(PaymentProcessingMessage message)
        {
            WasProcessMessageCalled = true;
            ProcessMessageCallCount++;
            PassedInMessage = message;

            if (message.PaymentProvider == "Error")
                throw new Exception("error");
        }
    }
}
