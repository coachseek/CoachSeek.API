using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;

namespace CoachSeek.WebUI.Tests.Unit.Fakes
{
    public class MockCoachUpdateUseCase : ICoachUpdateUseCase
    {
        public bool WasUpdateCoachCalled;
        public CoachUpdateCommand Command;
        public Response Response;

        public Guid BusinessId { get; set; }

        public Response UpdateCoach(CoachUpdateCommand command)
        {
            WasUpdateCoachCalled = true;
            Command = command;

            return Response;
        }
    }
}
