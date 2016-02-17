using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Application.UseCases
{
    public class DiscountCodeUpdateUseCase : BaseUseCase, IDiscountCodeUpdateUseCase
    {
        public async Task<IResponse> UpdateDiscountCodeAsync(DiscountCodeUpdateCommand command)
        {
            try
            {
                var discountCode = new DiscountCode(command);
                await ValidateUpdateAsync(discountCode);
                await BusinessRepository.UpdateDiscountCodeAsync(Business.Id, discountCode);
                return new Response(discountCode.ToData());
            }
            catch (CoachseekException ex)
            {
                return HandleException(ex);
            }
        }

        private async Task ValidateUpdateAsync(DiscountCode discountCode)
        {
            var existingCode = await LookupDiscountCodeAsync(discountCode);
            if (existingCode.IsNotFound())
                throw new DiscountCodeInvalid(discountCode.Id);
        }

        private async Task<DiscountCodeData> LookupDiscountCodeAsync(DiscountCode discountCode)
        {
            return await BusinessRepository.GetDiscountCodeAsync(Business.Id, discountCode.Id);
        }
    }
}
