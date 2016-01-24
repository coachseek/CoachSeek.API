using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ICustomerCustomFieldValuesUpdateUseCase : IApplicationContextSetter
    {
        Task<IResponse> UpdateCustomerCustomFieldValuesAsync(CustomFieldValueListUpdateCommand command);
    }
}
