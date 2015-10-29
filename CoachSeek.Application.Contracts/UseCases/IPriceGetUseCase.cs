using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IPriceGetUseCase : IApplicationContextSetter
    {
        Task<IResponse> GetPriceAsync(PriceGetCommand command);
    }
}
