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

        public override string ToString()
        {
            return string.Format("Provider: {0}; Contents: {1}", PaymentProvider, Contents);
        }

        public string Id
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}
