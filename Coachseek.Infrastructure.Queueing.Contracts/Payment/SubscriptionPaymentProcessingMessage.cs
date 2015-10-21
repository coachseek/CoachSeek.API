//namespace Coachseek.Infrastructure.Queueing.Contracts.Payment
//{
//    public class SubscriptionPaymentProcessingMessage : PaymentProcessingMessage
//    {
//        // Testing constructor
//        public SubscriptionPaymentProcessingMessage(string id, string paymentProvider, string contents) 
//            : base(id, paymentProvider, contents)
//        { }

//        public SubscriptionPaymentProcessingMessage(string id, string payloadString) 
//            : base(id, payloadString)
//        { }

//        public static SubscriptionPaymentProcessingMessage Create(string paymentProvider, string contents)
//        {
//            return new SubscriptionPaymentProcessingMessage(null, paymentProvider, contents);
//        }

//        public override string ToString()
//        {
//            return string.Format("Provider:{0}|Contents:{1}", PaymentProvider, Contents);
//        }
//    }
//}
