using System.Collections.Generic;

namespace Coachseek.Infrastructure.Queueing.Contracts.Emailing
{
    public class BouncedEmailMessage : IMessage
    {
        public string Id { get; private set; }
        public string BounceType { get; private set; }
        public IEnumerable<string> Recipients { get; private set; }


        public BouncedEmailMessage(string id, string bounceType, IEnumerable<string> recipients)
        {
            Id = id;
            BounceType = bounceType;
            Recipients = recipients;
        }
    }
}
