using System.Threading.Tasks;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;

namespace Coachseek.Integration.Contracts.Payments.Interfaces
{
    public interface IPaymentProviderApi
    {
        string SandboxUrl { get; }
        string LiveUrl { get; }

        Task<bool> VerifyPaymentAsync(PaymentProcessingMessage message);
        bool VerifyPayment(PaymentProcessingMessage message);
    }
}
