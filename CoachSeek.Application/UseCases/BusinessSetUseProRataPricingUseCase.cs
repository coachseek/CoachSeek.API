using System;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.UseCases
{
    public class BusinessSetUseProRataPricingUseCase : BaseUseCase, IBusinessSetUseProRataPricingUseCase
    {
        public async Task<IResponse> SetUseProRataPricingAsync(BusinessSetUseProRataPricingCommand command)
        {
            await Context.BusinessContext.BusinessRepository.SetUseProRataPricingAsync(Business.Id, command.UseProRataPricing);
            return new Response();
        }
    }
}
