using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.WebUI.Tests.Unit.Fakes
{
    public class MockLocationUpdateUseCase : ILocationUpdateUseCase
    {
        public bool WasUpdateLocationCalled;
        public LocationUpdateCommand Command;
        public Response<LocationData> Response;


        public Response<LocationData> UpdateLocation(LocationUpdateCommand command)
        {
            WasUpdateLocationCalled = true;
            Command = command;

            return Response;
        }
    }
}
