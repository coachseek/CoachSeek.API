using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;

namespace Coachseek.Infrastructure.Queueing.Tests.Unit.Fakes
{
    public class StubPaymentProcessingQueueClient : IPaymentProcessingQueueClient
    {
        public void PushPaymentProcessingMessageOntoQueue(PaymentProcessingMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
