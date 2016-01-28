using System;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.UseCases
{
    public class CustomFieldTemplateGetByIdUseCase : BaseUseCase, ICustomFieldTemplateGetByIdUseCase
    {
        public async Task<CustomFieldTemplateData> GetCustomFieldTemplateAsync(Guid id)
        {
            return await BusinessRepository.GetCustomFieldTemplateAsync(Business.Id, id);
        }
    }
}
