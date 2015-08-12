using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IBusinessRegistrationUseCase
    {
        IUserRepository UserRepository { set; }
        IBusinessRepository BusinessRepository { set; }
        ISupportedCurrencyRepository SupportedCurrencyRepository { set; }

        IResponse RegisterBusiness(BusinessRegistrationCommand command);
    }
}
