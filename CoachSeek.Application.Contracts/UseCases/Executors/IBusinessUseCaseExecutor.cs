using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Contracts;

namespace CoachSeek.Application.Contracts.UseCases.Executors
{
    public interface IBusinessUseCaseExecutor
    {
        Task<IResponse> ExecuteForAsync<T>(T command, ApplicationContext context) where T : ICommand;
    }
}
