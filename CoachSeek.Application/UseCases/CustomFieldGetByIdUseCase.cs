using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.UseCases
{
    public class CustomFieldGetByIdUseCase : BaseUseCase, ICustomFieldGetByIdUseCase
    {
        public async Task<CustomFieldTemplateData> GetCustomFieldAsync(Guid id)
        {
            return await BusinessRepository.GetCustomFieldTemplateAsync(Business.Id, id);
        }
    }
}
