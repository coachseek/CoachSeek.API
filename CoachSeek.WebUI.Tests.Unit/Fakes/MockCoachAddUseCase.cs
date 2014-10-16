using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;

namespace CoachSeek.WebUI.Tests.Unit.Fakes
{
    public class MockCoachAddUseCase : ICoachAddUseCase
    {
        public bool WasAddCoachCalled;
        public CoachAddCommand Command;
        public CoachAddResponse Response;


        public CoachAddResponse AddCoach(CoachAddCommand command)
        {
            WasAddCoachCalled = true;
            Command = command;

            return Response;
        }
    }
}
