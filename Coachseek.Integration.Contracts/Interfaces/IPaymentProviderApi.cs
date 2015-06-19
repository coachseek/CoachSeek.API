using Coachseek.Infrastructure.Queueing.Contracts.Payment;

namespace Coachseek.Integration.Contracts.Interfaces
{
    public interface IPaymentProviderApi
    {
        string SandboxUrl { get; }
        string LiveUrl { get; }

        bool VerifyPayment(PaymentProcessingMessage message);
    }
}
