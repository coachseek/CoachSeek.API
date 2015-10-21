//using System;

//namespace Coachseek.Infrastructure.Queueing.Contracts.Payment
//{
//    public class OnlinePaymentProcessingMessage : PaymentProcessingMessage
//    {
//        // Testing constructor
//        public OnlinePaymentProcessingMessage(string id, string paymentProvider, string contents) 
//            : base(id, paymentProvider, contents)
//        { }

//        public OnlinePaymentProcessingMessage(string id, string payloadString) 
//            : base(id, payloadString)
//        { }

//        public static OnlinePaymentProcessingMessage Create(string paymentProvider, string contents)
//        {
//            return new OnlinePaymentProcessingMessage(null, paymentProvider, contents);
//        }
//    }
//}
