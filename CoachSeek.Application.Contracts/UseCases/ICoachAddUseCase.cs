using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ICoachAddUseCase : IApplicationContextSetter
    {
        Response AddCoach(CoachAddCommand command);
    }
}
