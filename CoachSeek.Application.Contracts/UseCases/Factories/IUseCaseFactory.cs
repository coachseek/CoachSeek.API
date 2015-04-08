﻿using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Contracts;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.Contracts.UseCases.Factories
{
    public interface IUseCaseFactory
    {
        Response ExecuteFor<T>(T command, IBusinessRepository businessRepository, Guid? businessId) where T : ICommand;
    }
}