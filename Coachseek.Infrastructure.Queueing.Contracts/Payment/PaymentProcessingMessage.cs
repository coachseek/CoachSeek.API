using System;

namespace Coachseek.Infrastructure.Queueing.Contracts.Payment
{
    public class PaymentProcessingMessage : IMessage
    {
        public string PaymentProvider { get; private set; }
        public string Contents { get; private set; }


        public PaymentProcessingMessage(string paymentProvider, string contents)
        {
            PaymentProvider = paymentProvider;
            Contents = contents;
        }

        public PaymentProcessingMessage(string payloadString)
        {
            if (payloadString == null)
                throw new ArgumentNullException("payloadString");

            var parts = payloadString.Split('|');
            PaymentProvider = parts[0].Split(':')[1];
            Contents = parts[1].Split(':')[1];
        }

        public override string ToString()
        {
            return string.Format("Provider:{0}|Contents:{1}", PaymentProvider, Contents);
        }

        public string Id
        {
            get { throw new NotImplementedException(); }
        }
    }
}
