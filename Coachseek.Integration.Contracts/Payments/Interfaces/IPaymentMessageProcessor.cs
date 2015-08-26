using Coachseek.Infrastructure.Queueing.Contracts.Payment;

namespace Coachseek.Integration.Contracts.Payments.Interfaces
{
    public interface IPaymentMessageProcessor
    {
        void ProcessMessage(PaymentProcessingMessage message);
    }
}
