using System;
using System.Collections.Generic;

namespace Coachseek.Infrastructure.Queueing.Contracts
{
    public interface IQueueClient<TMessage> : IDisposable 
    {
        Queue GetQueue(string queueName);
        void Push(Queue queue, TMessage message);
        IList<TMessage> Peek(Queue queue, int maxCount = 10);
        void Pop(Queue queue, TMessage message);
    }
}
