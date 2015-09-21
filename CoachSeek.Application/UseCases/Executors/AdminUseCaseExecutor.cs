﻿using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Application.Contracts.UseCases.Executors;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Contracts;

namespace CoachSeek.Application.UseCases.Executors
{
    public class AdminUseCaseExecutor : IAdminUseCaseExecutor
    {
        public IBusinessSetAuthorisedUntilUseCase BusinessSetAuthorisedUntilUseCase { get; set; }

        public AdminUseCaseExecutor(IBusinessSetAuthorisedUntilUseCase businessSetAuthorisedUntilUseCase)
        {
            BusinessSetAuthorisedUntilUseCase = businessSetAuthorisedUntilUseCase;
        }


        public IResponse ExecuteFor<T>(T command, AdminApplicationContext context) where T : ICommand
        {
            if (command.GetType() == typeof(BusinessSetAuthorisedUntilCommand))
            {
                BusinessSetAuthorisedUntilUseCase.Initialise(context);
                return BusinessSetAuthorisedUntilUseCase.SetAuthorisedUntil(command as BusinessSetAuthorisedUntilCommand);
            }

            throw new NotImplementedException();
        }
    }
}