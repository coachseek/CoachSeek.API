using System;
using System.Collections.Generic;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;

namespace Coachseek.Infrastructure.Queueing.Tests.Unit.Fakes
{
    public class StubPaymentProcessingQueueClient : IPaymentProcessingQueueClient
    {
        public void Push(PaymentProcessingMessage message)
        {
            throw new NotImplementedException();
        }

        public IList<PaymentProcessingMessage> Peek()
        {
            throw new NotImplementedException();
        }

        public void Pop(PaymentProcessingMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
