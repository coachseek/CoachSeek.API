using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IBusinessAddUseCase
    {
        IBusinessRepository BusinessRepository { set; }
        ISupportedCurrencyRepository SupportedCurrencyRepository { set; }

        Task<IResponse> AddBusinessAsync(BusinessRegistrationCommand registration);
    }
}
