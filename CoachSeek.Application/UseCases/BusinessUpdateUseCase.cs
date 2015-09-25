using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Application.UseCases
{
    public class BusinessUpdateUseCase : BaseUseCase, IBusinessUpdateUseCase
    {
        public async Task<IResponse> UpdateBusinessAsync(BusinessUpdateCommand command)
        {
            try
            {
                var existingBusiness = await BusinessRepository.GetBusinessAsync(Business.Id);
                var business = new Business(existingBusiness, command, SupportedCurrencyRepository);
                await BusinessRepository.UpdateBusinessAsync(business);
                return new Response(business.ToData());
            }
            catch (CoachseekException ex)
            {
                return HandleException(ex);
            }
        }
    }
}