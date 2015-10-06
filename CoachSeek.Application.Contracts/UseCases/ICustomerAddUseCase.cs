using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ICustomerAddUseCase : IApplicationContextSetter
    {
        Task<IResponse> AddCustomerAsync(CustomerAddCommand command);
    }
}
