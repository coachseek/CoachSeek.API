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

        public IResponse AddBusiness(BusinessRegistrationCommand command)
        {
            BusinessDomainBuilder.BusinessRepository = BusinessRepository;
            var newBusiness = new NewBusiness(command, BusinessDomainBuilder, SupportedCurrencyRepository);
            BusinessRepository.AddBusiness(newBusiness);
            return new Response(newBusiness.ToData());
        }
    }
}