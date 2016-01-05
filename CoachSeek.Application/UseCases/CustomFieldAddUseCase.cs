using System;
using System.Linq;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Application.UseCases
{
    public class CustomFieldAddUseCase : BaseUseCase, ICustomFieldAddUseCase
    {
        public async Task<IResponse> AddCustomFieldAsync(CustomFieldAddCommand command)
        {
            try
            {
                var newTemplate = new CustomFieldTemplate(command);
                await ValidateAddAsync(newTemplate);
                await BusinessRepository.AddCustomFieldTemplateAsync(Business.Id, newTemplate);
                return new Response(newTemplate.ToData());
            }
            catch (CoachseekException ex)
            {
                return HandleException(ex);
            }
        }

        private async Task ValidateAddAsync(CustomFieldTemplate newTemplate)
        {
            var existingTemplate = await LookupCustomFieldTemplateAsync(newTemplate);
            if (existingTemplate.IsFound())
                throw new CustomFieldTemplateDuplicate(newTemplate);
        }

        private async Task<CustomFieldTemplateData> LookupCustomFieldTemplateAsync(CustomFieldTemplate newTemplate)
        {
            var templates = await BusinessRepository.GetCustomFieldTemplatesAsync(Business.Id, newTemplate.Type, newTemplate.Key);
            return templates.SingleOrDefault();
        }
    }
}
