using System;
using System.Collections.Generic;

namespace Coachseek.Infrastructure.Queueing.Contracts
{
    public interface IQueueClient<TMessage> : IDisposable where TMessage : IMessage
    {
        IList<TMessage> GetMessages(Queue queue);
        void PopMessageFromQueue(TMessage message, Queue queue);
    }
}
