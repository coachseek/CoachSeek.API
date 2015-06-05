using System;
using System.Collections.Generic;

namespace Coachseek.Infrastructure.Queueing.Contracts
{
    public interface IQueueClient<TMessage> : IDisposable 
    {
        Queue GetQueue(string queueName);
        void PushMessageOntoQueue(Queue queue, TMessage message);
        IList<TMessage> GetMessages(Queue queue);
        void PopMessageFromQueue(Queue queue, TMessage message);
    }
}
