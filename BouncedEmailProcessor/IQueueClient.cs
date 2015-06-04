using System;
using System.Collections.Generic;

namespace BouncedEmailProcessor
{
    public interface IQueueClient<TMessage> : IDisposable where TMessage : IMessage
    {
        IList<TMessage> GetMessages(Queue queue);
        void PopMessageFromQueue(TMessage message, Queue queue);
    }
}
