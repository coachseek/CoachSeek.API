using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Contracts;

namespace CoachSeek.Application.Contracts.UseCases.Executors
{
    public interface IAdminUseCaseExecutor
    {
        IResponse ExecuteFor<T>(T command, AdminApplicationContext context) where T : ICommand;
    }
}