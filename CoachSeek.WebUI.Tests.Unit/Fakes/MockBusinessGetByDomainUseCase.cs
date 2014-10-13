using CoachSeek.WebUI.Contracts.UseCases;
using CoachSeek.WebUI.Models.UseCases.Responses;

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
