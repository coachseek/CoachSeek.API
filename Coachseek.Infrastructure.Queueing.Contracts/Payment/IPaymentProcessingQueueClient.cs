using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coachseek.Infrastructure.Queueing.Contracts.Payment
{
    public interface IPaymentProcessingQueueClient
    {
        Task PushAsync(PaymentProcessingMessage message);
        Task<IList<PaymentProcessingMessage>> PeekAsync();
        Task PopAsync(PaymentProcessingMessage message);
    }
}
