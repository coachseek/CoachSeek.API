using Coachseek.Infrastructure.Queueing.Contracts.Payment;

namespace Coachseek.Integration.Contracts.Interfaces
{
    public interface IPaymentMessageProcessor
    {
        void ProcessMessage(PaymentProcessingMessage message);
    }
}
