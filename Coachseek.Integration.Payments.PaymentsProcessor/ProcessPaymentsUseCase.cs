using System;
using CoachSeek.Domain.Repositories;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;

namespace Coachseek.Integration.Payments.PaymentsProcessor
{
    public class ProcessPaymentsUseCase
    {
        private bool IsPaymentEnabled { get; set; }
        private IPaymentProcessingQueueClient PaymentProcessingQueueClient { get; set; }
        private IPaymentRepository PaymentsRepository { get; set; }

        public ProcessPaymentsUseCase(IPaymentProcessingQueueClient paymentProcessingQueueClient, 
                                      IPaymentRepository paymentsRepository,
                                      bool isPaymentEnabled)
        {
            PaymentProcessingQueueClient = paymentProcessingQueueClient;
            PaymentsRepository = paymentsRepository;
            IsPaymentEnabled = isPaymentEnabled;
        }


        public void Process()
        {
            var messages = PaymentProcessingQueueClient.Peek();

            foreach (var message in messages)
            {
                ProcessMessage(message);

                // Save payment to the database.

                // Verify payment with PayPal

                // Other checks...

                // check that Payment_status=Completed
                // check that Txn_id has not been previously processed
                // check that Receiver_email is your Primary PayPal email
                // check that Payment_amount/Payment_currency are correct
                // process payment

                PaymentProcessingQueueClient.Pop(message);
            }
        }

        private void ProcessMessage(PaymentProcessingMessage message)
        {
            try
            {
                TryProcessMessage(message);
            }
            catch (Exception ex)
            {
                
            }
        }

        private void TryProcessMessage(PaymentProcessingMessage message)
        {
            var payment = PaymentConverter.Convert(message);
            PaymentsRepository.AddPayment(payment);

            var paymentApi = PaymentProviderApiFactory.GetPaymentProviderApi(message, IsPaymentEnabled);
            paymentApi.VerifyPayment(message);
        }
    }
}
