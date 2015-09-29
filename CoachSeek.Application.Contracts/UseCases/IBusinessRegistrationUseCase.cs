using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IBusinessRegistrationUseCase
    {
        bool IsTesting { set; }
        bool IsUserTrackingEnabled { set; }
        string UserTrackerCredentials { set; }
        IUserRepository UserRepository { set; }
        IBusinessRepository BusinessRepository { set; }
        ISupportedCurrencyRepository SupportedCurrencyRepository { set; }

        Task<IResponse> RegisterBusinessAsync(BusinessRegistrationCommand command);
    }
}
