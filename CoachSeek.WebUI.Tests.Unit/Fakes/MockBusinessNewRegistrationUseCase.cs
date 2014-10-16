using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;

namespace CoachSeek.WebUI.Tests.Unit.Fakes
{
    public class MockBusinessNewRegistrationUseCase : IBusinessNewRegistrationUseCase
    {
        public bool WasRegisterNewBusinessCalled;
        public BusinessRegistrationCommand Command;
        public BusinessRegistrationResponse Response;


        public BusinessRegistrationResponse RegisterNewBusiness(BusinessRegistrationCommand registration)
        {
            WasRegisterNewBusinessCalled = true;
            Command = registration;
            return Response;
        }
    }
}
