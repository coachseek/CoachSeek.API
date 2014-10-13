using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.Contracts.UseCases;

namespace CoachSeek.WebUI.Tests.Unit.Fakes
{
    public class MockBusinessGetByDomainUseCase : IBusinessGetByDomainUseCase
    {
        public BusinessGetResponse Response; 

        public BusinessGetResponse GetByDomain(string domain)
        {
            return Response;
        }
    }
}
