﻿using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ILocationUpdateUseCase : IApplicationContextSetter
    {
        IResponse UpdateLocation(LocationUpdateCommand command);
    }
}
