using System.Collections.Generic;

namespace BouncedEmailProcessor
{
    public class BouncedEmailMessage : IMessage
    {
        public string ReceiptId { get; private set; }
        public string BounceType { get; private set; }
        public IEnumerable<string> Recipients { get; private set; }


        public BouncedEmailMessage(string receiptId, string bounceType, IEnumerable<string> recipients)
        {
            BounceType = bounceType;
            ReceiptId = receiptId;
            Recipients = recipients;
        }
    }
}
