using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;

namespace CoachSeek.WebUI.Tests.Unit.Fakes
{
    public class MockLocationUpdateUseCase : ILocationUpdateUseCase
    {
        public bool WasUpdateLocationCalled;
        public LocationUpdateResponse Response;


        public LocationUpdateResponse UpdateLocation(LocationUpdateRequest request)
        {
            WasUpdateLocationCalled = true;
            return Response;
        }
    }
}
