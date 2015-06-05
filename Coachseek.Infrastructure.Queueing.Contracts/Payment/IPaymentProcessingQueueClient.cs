namespace Coachseek.Infrastructure.Queueing.Contracts.Payment
{
    public interface IPaymentProcessingQueueClient
    {
        void PushPaymentProcessingMessageOntoQueue(PaymentProcessingMessage message);
    }
}
