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
    public class DiscountCodeAddUseCase : BaseUseCase, IDiscountCodeAddUseCase
    {
        public async Task<IResponse> AddDiscountCodeAsync(DiscountCodeAddCommand command)
        {
            try
            {
                var newCode = new DiscountCode(command);
                await ValidateAddAsync(newCode);
                await BusinessRepository.AddDiscountCodeAsync(Business.Id, newCode);
                return new Response(newCode.ToData());
            }
            catch (CoachseekException ex)
            {
                return HandleException(ex);
            }
        }

        private async Task ValidateAddAsync(DiscountCode newCode)
        {
            var existingCode = await LookupDiscountCodeAsync(newCode);
            if (existingCode.IsFound())
                throw new DiscountCodeDuplicate(newCode);
        }

        private async Task<DiscountCodeData> LookupDiscountCodeAsync(DiscountCode newCode)
        {
            return await BusinessRepository.GetDiscountCodeAsync(Business.Id, newCode.Code);
        }
    }
}
