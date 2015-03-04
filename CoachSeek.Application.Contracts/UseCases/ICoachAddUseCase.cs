﻿using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ICoachAddUseCase : IBusinessRepositorySetter
    {
        Response AddCoach(CoachAddCommand command);
    }
}
