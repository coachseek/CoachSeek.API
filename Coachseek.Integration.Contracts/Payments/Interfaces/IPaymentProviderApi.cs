using Coachseek.Infrastructure.Queueing.Contracts.Payment;

namespace Coachseek.Integration.Contracts.Payments.Interfaces
{
    public interface IPaymentProviderApi
    {
        string SandboxUrl { get; }
        string LiveUrl { get; }

        bool VerifyPayment(PaymentProcessingMessage message);
    }
}
