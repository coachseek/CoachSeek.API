using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IDiscountCodeGetAllUseCase : IApplicationContextSetter
    {
        Task<IList<DiscountCodeData>> GetDiscountCodesAsync();
    }
}
