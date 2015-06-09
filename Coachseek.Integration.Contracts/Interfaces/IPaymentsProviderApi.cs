using System.Security.Cryptography.X509Certificates;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;

namespace Coachseek.Integration.Contracts.Interfaces
{
    public interface IPaymentsProviderApi
    {
        string SandboxUrl { get; }
        string LiveUrl { get; }

        void VerifyPayment(PaymentProcessingMessage message);
    }
}
