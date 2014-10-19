using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.WebUI.Tests.Unit.Fakes
{
    public class MockLocationAddUseCase : ILocationAddUseCase
    {
        public bool WasAddLocationCalled;
        public LocationAddCommand Command;
        public Response<LocationData> Response;


        public Response<LocationData> AddLocation(LocationAddCommand command)
        {
            WasAddLocationCalled = true;
            Command = command;

            return Response;
        }
    }
}
