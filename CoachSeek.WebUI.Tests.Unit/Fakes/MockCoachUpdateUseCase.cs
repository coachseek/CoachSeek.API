﻿using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;

namespace CoachSeek.WebUI.Tests.Unit.Fakes
{
    public class MockCoachUpdateUseCase : ICoachUpdateUseCase
    {
        public bool WasUpdateCoachCalled;
        public CoachUpdateCommand Command;
        public Response Response;


        public Response UpdateCoach(CoachUpdateCommand command)
        {
            WasUpdateCoachCalled = true;
            Command = command;

            return Response;
        }
    }
}
