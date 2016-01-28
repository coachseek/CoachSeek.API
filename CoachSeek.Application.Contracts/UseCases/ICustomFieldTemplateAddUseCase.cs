using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ICustomFieldTemplateAddUseCase : IApplicationContextSetter
    {
        Task<IResponse> AddCustomFieldTemplateAsync(CustomFieldAddCommand command);
    }
}
