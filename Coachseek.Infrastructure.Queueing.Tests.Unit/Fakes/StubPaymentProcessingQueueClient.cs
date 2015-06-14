using System.Collections.Generic;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;

namespace Coachseek.Infrastructure.Queueing.Tests.Unit.Fakes
{
    public class StubPaymentProcessingQueueClient : IPaymentProcessingQueueClient
    {
        public bool WasPushCalled;
        public int PushCallCount = 0;
        public bool WasPopCalled;
        public int PopCallCount = 0;
        public PaymentProcessingMessage PassedInMessage;
        public List<PaymentProcessingMessage> Messages;


        public StubPaymentProcessingQueueClient()
        {
            Messages = new List<PaymentProcessingMessage>();
        }

        public StubPaymentProcessingQueueClient(PaymentProcessingMessage message)
        {
            Messages = new List<PaymentProcessingMessage> {message};
        }

        public StubPaymentProcessingQueueClient(IEnumerable<PaymentProcessingMessage> messages)
        {
            Messages = new List<PaymentProcessingMessage>(messages);
        }


        public void Push(PaymentProcessingMessage message)
        {
            WasPushCalled = true;
            PushCallCount++;
            PassedInMessage = message;
        }

        public IList<PaymentProcessingMessage> Peek()
        {
            return Messages;
        }

        public void Pop(PaymentProcessingMessage message)
        {
            WasPopCalled = true;
            PopCallCount++;
            PassedInMessage = message;
        }
    }
}
