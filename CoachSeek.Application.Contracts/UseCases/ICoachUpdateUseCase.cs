using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ICoachUpdateUseCase
    {
        Guid BusinessId { set; }

        Response UpdateCoach(CoachUpdateCommand command);
    }
}
