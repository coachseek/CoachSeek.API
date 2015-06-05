using System;
using System.Collections.Generic;

namespace Coachseek.Infrastructure.Queueing.Amazon.Models
{
    public class AmazonSesBounce
    {
        public string BounceType { get; set; }
        public string BounceSubType { get; set; }
        public DateTime Timestamp { get; set; }
        public List<AmazonSesBouncedRecipient> BouncedRecipients { get; set; }
    }
}
