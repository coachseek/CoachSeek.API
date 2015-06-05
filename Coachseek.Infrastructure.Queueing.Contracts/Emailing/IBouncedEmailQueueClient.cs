using System;
using System.Collections.Generic;

namespace Coachseek.Infrastructure.Queueing.Contracts.Emailing
{
    public interface IBouncedEmailQueueClient : IDisposable
    {
        IList<BouncedEmailMessage> Peek();
        void Pop(BouncedEmailMessage message);
    }
}
