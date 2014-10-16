﻿using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;

namespace CoachSeek.WebUI.Tests.Unit.Fakes
{
    public class MockLocationAddUseCase : ILocationAddUseCase
    {
        public bool WasAddLocationCalled;
        public LocationAddCommand Command;
        public LocationAddResponse Response;


        public LocationAddResponse AddLocation(LocationAddCommand command)
        {
            WasAddLocationCalled = true;
            Command = command;

            return Response;
        }
    }
}
