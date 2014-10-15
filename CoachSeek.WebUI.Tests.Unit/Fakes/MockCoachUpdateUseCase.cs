using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;

namespace CoachSeek.WebUI.Tests.Unit.Fakes
{
    public class MockCoachUpdateUseCase : ICoachUpdateUseCase
    {
        public bool WasUpdateCoachCalled;
        public CoachUpdateResponse Response;


        public CoachUpdateResponse UpdateCoach(CoachUpdateCommand request)
        {
            WasUpdateCoachCalled = true;
            return Response;
        }
    }
}
