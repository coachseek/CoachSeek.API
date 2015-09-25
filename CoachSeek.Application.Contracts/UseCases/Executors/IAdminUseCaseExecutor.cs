using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Contracts;

namespace CoachSeek.Application.Contracts.UseCases.Executors
{
    public interface IAdminUseCaseExecutor
    {
        Task<IResponse> ExecuteForAsync<T>(T command, AdminApplicationContext context) where T : ICommand;
    }
}