using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Contracts;

namespace CoachSeek.Application.Contracts.UseCases.Executors
{
    public interface ICustomFieldUseCaseExecutor
    {
        Task<IResponse> ExecuteForAsync<T>(T command, ApplicationContext context) where T : ICommand;
    }
}
