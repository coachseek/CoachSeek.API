﻿using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ILocationAddUseCase
    {
        Response AddLocation(LocationAddCommand command);
    }
}
