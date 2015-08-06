using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Contracts;

namespace CoachSeek.Application.Contracts.UseCases.Executors
{
    public interface IBookingUseCaseExecutor
    {
        Response ExecuteFor<T>(T command, ApplicationContext context) where T : ICommand;
    }
}