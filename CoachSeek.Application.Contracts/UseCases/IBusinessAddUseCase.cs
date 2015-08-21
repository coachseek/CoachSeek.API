using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IBusinessAddUseCase
    {
        IBusinessRepository BusinessRepository { set; }
        ISupportedCurrencyRepository SupportedCurrencyRepository { set; }

        IResponse AddBusiness(BusinessRegistrationCommand registration);
    }
}
