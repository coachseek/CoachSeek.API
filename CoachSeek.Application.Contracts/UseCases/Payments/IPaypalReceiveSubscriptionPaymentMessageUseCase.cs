using System.Threading.Tasks;

namespace CoachSeek.Application.Contracts.UseCases.Payments
{
    public interface IPaypalReceiveSubscriptionPaymentMessageUseCase
    {
        Task ReceiveAsync(string formData);
    }
}
