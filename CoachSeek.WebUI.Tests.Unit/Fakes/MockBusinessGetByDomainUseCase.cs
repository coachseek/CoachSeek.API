using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;

namespace CoachSeek.WebUI.Tests.Unit.Fakes
{
    public class MockBusinessGetByDomainUseCase : IBusinessGetByDomainUseCase
    {
        public string Domain;
        public BusinessData Response; 

        public BusinessData GetByDomain(string domain)
        {
            Domain = domain;

            return Response;
        }
    }
}
