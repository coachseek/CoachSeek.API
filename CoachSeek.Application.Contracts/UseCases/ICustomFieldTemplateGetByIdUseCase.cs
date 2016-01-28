using System;
using System.Threading.Tasks;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ICustomFieldTemplateGetByIdUseCase : IApplicationContextSetter
    {
        Task<CustomFieldTemplateData> GetCustomFieldTemplateAsync(Guid id);
    }
}
