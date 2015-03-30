﻿using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.WebUI.Tests.Unit.Fakes
{
    public class MockCoachAddUseCase : ICoachAddUseCase
    {
        public bool WasAddCoachCalled;
        public CoachAddCommand Command;
        public Response Response;

        public Guid BusinessId { get; set; }
        public IBusinessRepository BusinessRepository { get; set; }


        public void Initialise(IBusinessRepository businessRepository, Guid? businessId = null)
        {
        }

        public Response AddCoach(CoachAddCommand command)
        {
            WasAddCoachCalled = true;
            Command = command;

            return Response;
        }
    }
}
