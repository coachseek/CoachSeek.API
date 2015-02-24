using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;

namespace CoachSeek.WebUI.Tests.Unit.Fakes
{
    public class MockCoachAddUseCase : ICoachAddUseCase
    {
        public bool WasAddCoachCalled;
        public CoachAddCommand Command;
        public Response Response;

        public Guid BusinessId { get; set; }

        public Response AddCoach(CoachAddCommand command)
        {
            WasAddCoachCalled = true;
            Command = command;

            return Response;
        }
    }
}
