using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.WebUI.Tests.Unit.Fakes
{
    public class MockBusinessNewRegistrationUseCase : IBusinessNewRegistrationUseCase
    {
        public bool WasRegisterNewBusinessCalled;
        public BusinessRegistrationCommand Command;
        public Response<BusinessData> Response;


        public Response<BusinessData> RegisterNewBusiness(BusinessRegistrationCommand registration)
        {
            WasRegisterNewBusinessCalled = true;
            Command = registration;
            return Response;
        }
    }
}
