using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ISessionAddUseCase : IApplicationContextSetter
    {
        Response AddSession(SessionAddCommand command);
    }
}
