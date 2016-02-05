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
    public class CustomFieldTemplateUpdateUseCase : BaseUseCase, ICustomFieldTemplateUpdateUseCase
    {
        public async Task<IResponse> UpdateCustomFieldTemplateAsync(CustomFieldUpdateCommand command)
        {
            try
            {
                var template = new CustomFieldTemplate(command);
                await ValidateUpdateAsync(template);
                await BusinessRepository.UpdateCustomFieldTemplateAsync(Business.Id, template);
                return new Response(template.ToData());
            }
            catch (CoachseekException ex)
            {
                return HandleException(ex);
            }
        }

        private async Task ValidateUpdateAsync(CustomFieldTemplate template)
        {
            var existingTemplate = await BusinessRepository.GetCustomFieldTemplateAsync(Business.Id, template.Id);
            if (existingTemplate.IsNotFound())
                throw new CustomFieldTemplateIdInvalid(template.Id);
            var templateWithSameKey = await LookupCustomFieldTemplateAsync(template);
            if (templateWithSameKey.IsFound() && templateWithSameKey.Id != template.Id)
                throw new CustomFieldTemplateDuplicate(template);
        }

        private async Task<CustomFieldTemplateData> LookupCustomFieldTemplateAsync(CustomFieldTemplate template)
        {
            var templates = await BusinessRepository.GetCustomFieldTemplatesAsync(Business.Id, template.Type, template.Key);
            return templates.SingleOrDefault();
        }



        //private async Task ValidateAddAsync(CustomFieldTemplate newTemplate)
        //{
        //    var existingTemplate = await LookupCustomFieldTemplateAsync(newTemplate);
        //    if (existingTemplate.IsFound())
        //        throw new CustomFieldTemplateDuplicate(newTemplate);
        //}

    }
}
