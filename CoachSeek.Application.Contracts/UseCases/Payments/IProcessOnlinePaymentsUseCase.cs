using System.Threading.Tasks;

namespace CoachSeek.Application.Contracts.UseCases.Payments
{
    public interface IProcessOnlinePaymentsUseCase
    {
        Task ProcessAsync();
    }
}
