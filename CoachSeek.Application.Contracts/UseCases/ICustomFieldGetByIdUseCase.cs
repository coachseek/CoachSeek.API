using System;
using System.Threading.Tasks;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ICustomFieldGetByIdUseCase : IApplicationContextSetter
    {
        Task<CustomFieldTemplateData> GetCustomFieldAsync(Guid id);
    }
}
