using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Contracts;

namespace CoachSeek.Application.Contracts.UseCases.Executors
{
    public interface IBookingUseCaseExecutor
    {
        IResponse ExecuteFor<T>(T command, ApplicationContext context) where T : ICommand;
    }
}