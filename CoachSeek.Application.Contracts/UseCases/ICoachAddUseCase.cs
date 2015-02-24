using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ICoachAddUseCase
    {
        Guid BusinessId { set; }

        Response AddCoach(CoachAddCommand command);
    }
}
