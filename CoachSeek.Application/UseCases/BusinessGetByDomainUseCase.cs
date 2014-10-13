using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.UseCases
{
    public class BusinessGetByDomainUseCase : IBusinessGetByDomainUseCase
    {
        private IBusinessRepository BusinessRepository { get; set; }


        public BusinessGetByDomainUseCase(IBusinessRepository businessRepository)
        {
            BusinessRepository = businessRepository;
        }


        public BusinessGetResponse GetByDomain(string domain)
        {
            if (string.IsNullOrWhiteSpace(domain))
                return new NoDomainBusinessGetResponse();

            var business = BusinessRepository.GetByDomain(domain);
            return new BusinessGetResponse(business);
        }
    }
}