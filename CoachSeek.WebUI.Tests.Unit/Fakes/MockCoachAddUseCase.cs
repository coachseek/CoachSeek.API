using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.WebUI.Tests.Unit.Fakes
{
    public class MockCoachAddUseCase : ICoachAddUseCase
    {
        public bool WasAddCoachCalled;
        public CoachAddCommand Command;
        public Response<CoachData> Response;


        public Response<CoachData> AddCoach(CoachAddCommand command)
        {
            WasAddCoachCalled = true;
            Command = command;

            return Response;
        }
    }
}
