using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.UseCases
{
    public class BusinessUpdateUseCase : BaseUseCase, IBusinessUpdateUseCase
    {
        private IReservedDomainRepository ReservedDomainRepository { get; set; }

        public BusinessUpdateUseCase(IReservedDomainRepository reservedDomainRepository)
        {
            ReservedDomainRepository = reservedDomainRepository;
        }


        public async Task<IResponse> UpdateBusinessAsync(BusinessUpdateCommand command)
        {
            try
            {
                var existingBusiness = await BusinessRepository.GetBusinessAsync(Business.Id);
                var updateBusiness = new Business(existingBusiness, command, SupportedCurrencyRepository);
                await ValidateUpdate(updateBusiness);
                await BusinessRepository.UpdateBusinessAsync(updateBusiness);
                return new Response(updateBusiness.ToData());
            }
            catch (CoachseekException ex)
            {
                return HandleException(ex);
            }
        }

        private async Task ValidateUpdate(Business updateBusiness)
        {
            var domainBusiness = await BusinessRepository.GetBusinessAsync(updateBusiness.Domain);
            if (domainBusiness != null && domainBusiness.Id != updateBusiness.Id)
                throw new SubdomainDuplicate(updateBusiness.Domain);
            if (ReservedDomainRepository.Contains(updateBusiness.Domain))
                throw new SubdomainDuplicate(updateBusiness.Domain);
        }
    }
}