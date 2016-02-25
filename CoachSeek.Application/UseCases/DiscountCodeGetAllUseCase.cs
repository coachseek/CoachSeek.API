using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.UseCases
{
    public class DiscountCodeGetAllUseCase :BaseUseCase, IDiscountCodeGetAllUseCase
    {
        public async Task<IList<DiscountCodeData>> GetDiscountCodesAsync()
        {
            return await BusinessRepository.GetDiscountCodesAsync(Business.Id);
        }
    }
}
