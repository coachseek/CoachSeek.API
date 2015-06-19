using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Entities;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;
using Coachseek.Integration.Contracts.Interfaces;

namespace Coachseek.Integration.Payments.PaymentsProcessor
{
    public class PaymentMessageProcessor : IPaymentMessageProcessor
    {
        public IPaymentProcessorConfiguration PaymentProcessorConfiguration { get; private set; }
        public IDataAccessFactory DataAccessFactory { get; private set; }
        public IPaymentProviderApiFactory PaymentProviderApiFactory { get; private set; }


        public PaymentMessageProcessor(IPaymentProcessorConfiguration paymentProcessorConfiguration,
                                       IDataAccessFactory dataAccessFactory,
                                       IPaymentProviderApiFactory paymentProviderApiFactory)
        {
            PaymentProcessorConfiguration = paymentProcessorConfiguration;
            DataAccessFactory = dataAccessFactory;
            PaymentProviderApiFactory = paymentProviderApiFactory;
        }


        public void ProcessMessage(PaymentProcessingMessage message)
        {
            var isVerified = AttemptMessageVerification(message);
            if (!isVerified)
                return;
            var newPayment = NewPaymentConverter.Convert(message);
            if (newPayment.IsPending)
                return;
            var dataAccess = DataAccessFactory.CreateDataAccess(newPayment.IsTesting);
            var payment = SaveIfNewPayment(newPayment, dataAccess);


            // Update the Booking with the correct payment status.
            //dataAccess.BusinessRepository.UpdateBookingPaymentStatus();



            // Validate payment.


            // Validation succeeds...
            // ...and send email to customer and business admin?
            // ...set payment status to 'paid'.


            // Other checks...

            // check that Payment_status=Completed
            // check that Txn_id has not been previously processed
            // check that Receiver_email is your Primary PayPal email
            // check that Payment_amount/Payment_currency are correct
            // process payment
        }

        private Payment SaveIfNewPayment(NewPayment newPayment, DataRepositories dataAccess)
        {
            var repository = dataAccess.TransactionRepository;
            var payment = repository.GetPayment(newPayment.Id);
            if (payment.IsNotFound())
            {
                repository.AddPayment(newPayment);
                payment = newPayment;
            }
            return payment;
        }

        private bool AttemptMessageVerification(PaymentProcessingMessage message)
        {
            var isPaymentEnabled = PaymentProcessorConfiguration.IsPaymentEnabled;
            var paymentApi = PaymentProviderApiFactory.GetPaymentProviderApi(message, isPaymentEnabled);
            return paymentApi.VerifyPayment(message);
        }
    }
}
