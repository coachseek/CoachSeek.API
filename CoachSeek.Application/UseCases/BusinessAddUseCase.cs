using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Contracts;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.UseCases
{
    public class BusinessAddUseCase : IBusinessAddUseCase
    {
        public IBusinessRepository BusinessRepository { set; private get; }
        public ISupportedCurrencyRepository SupportedCurrencyRepository { set; private get; }

        private IBusinessDomainBuilder BusinessDomainBuilder { get; set; }

        public BusinessAddUseCase(IBusinessDomainBuilder businessDomainBuilder)
        {
            BusinessDomainBuilder = businessDomainBuilder;
        }

        public async Task<IResponse> AddBusinessAsync(BusinessRegistrationCommand command)
        {
            var newBusiness = await CreateNewBusiness(command);
            await BusinessRepository.AddBusinessAsync(newBusiness);
            return new Response(newBusiness.ToData());
        }

        private async Task<NewBusiness> CreateNewBusiness(BusinessRegistrationCommand command)
        {
            BusinessDomainBuilder.BusinessRepository = BusinessRepository;
            return await NewBusiness.CreateAsync(command, BusinessDomainBuilder, SupportedCurrencyRepository);
        }
    }
}