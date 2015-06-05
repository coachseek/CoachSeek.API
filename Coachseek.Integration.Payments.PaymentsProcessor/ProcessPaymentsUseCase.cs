using CoachSeek.Domain.Repositories;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;

namespace Coachseek.Integration.Payments.PaymentsProcessor
{
    public class ProcessPaymentsUseCase
    {
        private IPaymentProcessingQueueClient PaymentProcessingQueueClient { get; set; }
        private IPaymentRepository PaymentsRepository { get; set; }

        public ProcessPaymentsUseCase(IPaymentProcessingQueueClient paymentProcessingQueueClient, IPaymentRepository paymentsRepository)
        {
            PaymentProcessingQueueClient = paymentProcessingQueueClient;
            PaymentsRepository = paymentsRepository;
        }


        public void Process()
        {
            var messages = PaymentProcessingQueueClient.Peek();

            foreach (var message in messages)
            {
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
    }
}
