//using System;
//using CoachSeek.Common.Extensions;
//using CoachSeek.Domain.Repositories;
//using Coachseek.Infrastructure.Queueing.Contracts.Payment;

//namespace Coachseek.Integration.Payments.PaymentsProcessor
//{
//    public class ProcessPaymentsUseCase
//    {
//        private bool IsPaymentEnabled { get; set; }
//        private IPaymentProcessingQueueClient PaymentProcessingQueueClient { get; set; }
//        private ITransactionRepository TransactionRepository { get; set; }

//        public ProcessPaymentsUseCase(IPaymentProcessingQueueClient paymentProcessingQueueClient,
//                                      ITransactionRepository transactionRepository,
//                                      bool isPaymentEnabled)
//        {
//            PaymentProcessingQueueClient = paymentProcessingQueueClient;
//            TransactionRepository = transactionRepository;
//            IsPaymentEnabled = isPaymentEnabled;
//        }


//        public void Process()
//        {
//            var messages = PaymentProcessingQueueClient.Peek();

//            foreach (var message in messages)
//            {
//                try
//                {
//                    ProcessMessage(message);
//                    PaymentProcessingQueueClient.Pop(message);
//                }
//                catch (Exception ex)
//                {

//                }

//                // Save payment to the database.

//                // Verify payment with PayPal

//                // Other checks...

//                // check that Payment_status=Completed
//                // check that Txn_id has not been previously processed
//                // check that Receiver_email is your Primary PayPal email
//                // check that Payment_amount/Payment_currency are correct
//                // process payment
//            }
//        }

//        private void ProcessMessage(PaymentProcessingMessage message)
//        {
//            // Save to payment to database.
//            var payment = PaymentConverter.Convert(message);
//            var existingPayment = TransactionRepository.GetPayment(payment.Id);
//            if (existingPayment.IsFound())
//                return;
//            var data = TransactionRepository.AddPayment(payment);

//            // Verify the payment with payment provider.
//            var paymentApi = PaymentProviderApiFactory.GetPaymentProviderApi(message, IsPaymentEnabled);
//            paymentApi.VerifyPayment(message);

//            // Validate payment.



//            // Validation succeeds...
//            // ...and send email to customer and business admin?
//            // ...set payment status to 'paid'.

//        }
//    }
//}
