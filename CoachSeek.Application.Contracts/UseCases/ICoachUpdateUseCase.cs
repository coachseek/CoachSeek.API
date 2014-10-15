using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ICoachUpdateUseCase
    {
        CoachUpdateResponse UpdateCoach(CoachUpdateCommand command);
    }
}
