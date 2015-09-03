using System;

namespace Coachseek.Infrastructure.Queueing.Contracts.Payment
{
    public class PaymentProcessingMessage : IMessage
    {
        public string Id { get; private set; }
        public string PaymentProvider { get; private set; }
        public string Contents { get; private set; }


        // Testing constructor
        public PaymentProcessingMessage(string id, string paymentProvider, string contents)
        {
            Id = id;
            PaymentProvider = paymentProvider;
            Contents = contents;
        }

        public PaymentProcessingMessage(string id, string payloadString)
        {
            if (payloadString == null)
                throw new ArgumentNullException("payloadString");

            Id = id;
            var parts = payloadString.Split('|');
            PaymentProvider = parts[0].Split(':')[1];
            Contents = parts[1].Split(':')[1];
        }

        public static PaymentProcessingMessage Create(string paymentProvider, string contents)
        {
            return new PaymentProcessingMessage(null, paymentProvider, contents);
        }

        public override string ToString()
        {
            return string.Format("Provider:{0}|Contents:{1}", PaymentProvider, Contents);
        }
    }
}
