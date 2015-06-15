using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Repositories;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;
using Coachseek.Integration.Contracts.Interfaces;

namespace Coachseek.Integration.Payments.PaymentsProcessor
{
    public class PaymentMessageProcessor : IPaymentMessageProcessor
    {
        public IPaymentProcessorConfiguration PaymentProcessorConfiguration { get; private set; }
        public ITransactionRepository TransactionRepository { get; private set; }


        public PaymentMessageProcessor(IPaymentProcessorConfiguration paymentProcessorConfiguration, 
                                       ITransactionRepository transactionRepository)
        {
            PaymentProcessorConfiguration = paymentProcessorConfiguration;
            TransactionRepository = transactionRepository;
        }


        public void ProcessMessage(PaymentProcessingMessage message)
        {
            // Save to payment to database.
            var newPayment = NewPaymentConverter.Convert(message);
            var payment = TransactionRepository.GetPayment(newPayment.Id);
            if (payment.IsNotFound())
            {
                TransactionRepository.AddPayment(newPayment);
                payment = newPayment;
            }

            // Verify the payment with payment provider.
            var isPaymentEnabled = PaymentProcessorConfiguration.IsPaymentEnabled;
            var paymentApi = PaymentProviderApiFactory.GetPaymentProviderApi(message, isPaymentEnabled);
            var isVerified = paymentApi.VerifyPayment(message);
            payment.Verify(isVerified);
            TransactionRepository.VerifyPayment(payment);


            // Validate payment.


            // Validation succeeds...
            // ...and send email to customer and business admin?
            // ...set payment status to 'paid'.


            // Save payment to the database.

            // Verify payment with PayPal

            // Other checks...

            // check that Payment_status=Completed
            // check that Txn_id has not been previously processed
            // check that Receiver_email is your Primary PayPal email
            // check that Payment_amount/Payment_currency are correct
            // process payment
        }
    }
}
