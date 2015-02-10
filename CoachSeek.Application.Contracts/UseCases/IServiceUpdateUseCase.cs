﻿using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IServiceUpdateUseCase
    {
        Response UpdateService(ServiceUpdateCommand command);
    }
}
