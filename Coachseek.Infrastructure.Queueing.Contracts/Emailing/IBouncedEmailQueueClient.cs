using System;
using System.Collections.Generic;

namespace Coachseek.Infrastructure.Queueing.Contracts.Emailing
{
    public interface IBouncedEmailQueueClient : IDisposable
    {
        IList<BouncedEmailMessage> GetBouncedEmailMessages();
        void PopBouncedEmailMessageFromQueue(BouncedEmailMessage message);
    }
}
