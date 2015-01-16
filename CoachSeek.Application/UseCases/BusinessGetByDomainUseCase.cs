using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
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


        public BusinessData GetByDomain(string domain)
        {
            if (string.IsNullOrWhiteSpace(domain))
                return null;

            var business = BusinessRepository.GetByDomain(domain);
            if (business == null)
                return null;

            return business.ToData();
        }
    }
}