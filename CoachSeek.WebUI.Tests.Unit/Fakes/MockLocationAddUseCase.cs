using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;

namespace CoachSeek.WebUI.Tests.Unit.Fakes
{
    public class MockLocationAddUseCase : ILocationAddUseCase
    {
        public bool WasAddLocationCalled;
        public LocationAddResponse Response; 


        public LocationAddResponse AddLocation(LocationAddRequest request)
        {
            WasAddLocationCalled = true;
            return Response;
        }
    }
}
