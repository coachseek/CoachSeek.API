using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IBusinessSetUseProRataPricingUseCase : IApplicationContextSetter
    {
        Task<IResponse> SetUseProRataPricingAsync(BusinessSetUseProRataPricingCommand command);
    }
}
