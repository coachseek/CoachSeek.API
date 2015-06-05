using System.Collections.Generic;

namespace Coachseek.Infrastructure.Queueing.Contracts.Payment
{
    public interface IPaymentProcessingQueueClient
    {
        void Push(PaymentProcessingMessage message);
        IList<PaymentProcessingMessage> Peek();
        void Pop(PaymentProcessingMessage message);
    }
}
