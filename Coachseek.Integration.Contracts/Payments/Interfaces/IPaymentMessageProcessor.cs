using System.Threading.Tasks;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;

namespace Coachseek.Integration.Contracts.Payments.Interfaces
{
    public interface IPaymentMessageProcessor
    {
        Task ProcessMessageAsync(PaymentProcessingMessage message);
    }
}
