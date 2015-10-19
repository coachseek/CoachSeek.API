using System.Threading.Tasks;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IProcessPaymentsUseCase
    {
        Task ProcessAsync();
    }
}
