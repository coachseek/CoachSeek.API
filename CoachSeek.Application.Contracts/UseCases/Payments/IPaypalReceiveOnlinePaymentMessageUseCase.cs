using System.Threading.Tasks;

namespace CoachSeek.Application.Contracts.UseCases.Payments
{
    public interface IPaypalReceiveOnlinePaymentMessageUseCase
    {
        Task ReceiveAsync(string formData);
    }
}
